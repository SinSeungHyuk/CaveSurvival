using Cysharp.Threading.Tasks;
using GooglePlayGames.BasicApi;
using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerStat
{
    public event Action<PlayerStat, float> OnHpChanged; 

    private Player player;
    private int hpRegenTimer;
    private int def;


    #region LEVEL & EXP
    public ReactiveProperty<int> Level { get; private set; } = new();
    public ReactiveProperty<int> CurrentExp { get; private set; } = new();
    public int Exp { get; private set; }
    #endregion


    #region STAT
    public float MaxHp { get; private set; }
    public float Hp { get; private set; }
    public int HpRegen { get; private set; }
    public int Defense {  get; private set; }
    //public float HpSteal { get; private set; }
    public int BonusDamage { get; private set; }
    public int MeleeDamage { get; private set; }
    public int RangeDamage { get; private set; }
    public float Speed { get; private set; }
    public int Dodge { get; private set; }
    public int PickUpRange { get; private set; }
    public int ExpBonus {  get; private set; }
    #endregion


    public void InitializePlayerStat(PlayerDetailsSO playerDetailsSO, Player player)
    {
        this.player = player;

        Exp = Settings.startExp;
        Level.Value = 1;
        Level.Subscribe(level => player.PlayerLevelUp.PlayerStat_OnLevelChanged(level));
        CurrentExp.Value = 0;
        //CurrentExp.Subscribe(exp => ExpChanged(exp));

        MaxHp = playerDetailsSO.Hp;
        Hp = MaxHp;
        HpRegen = playerDetailsSO.HpRegen;
        hpRegenTimer = (int)(5 / (1 + (HpRegen - 1) / 2f) * 1000f);
        //HpSteal = playerDetailsSO.HpSteal;
        Defense = playerDetailsSO.Defense;
        def = UtilitieHelper.CombatScaling(Defense);
        BonusDamage = playerDetailsSO.BonusDamage;
        MeleeDamage = playerDetailsSO.MeleeDamage;
        RangeDamage = playerDetailsSO.RangeDamage;
        Speed = playerDetailsSO.Speed;
        Dodge = playerDetailsSO.Dodge;
        PickUpRange = playerDetailsSO.PickUpRange;
        ExpBonus = playerDetailsSO.ExpBonus;

        HPRegenRoutine().Forget();
    }

    
    public void TakeDamage(float damage)
    {
        // 회피 검사
        if (UtilitieHelper.isSuccess(Dodge))
        {
            HitTextUI dodgeText = ObjectPoolManager.Instance.Get(EPool.HitText, new Vector2(player.transform.position.x, player.transform.position.y + 1f), Quaternion.identity).GetComponent<HitTextUI>();
            dodgeText.InitializeHitText(0,EHitType.Dodge);

            return;
        }

        int dmg = ((int)UtilitieHelper.DecreaseByPercent(damage, def));

        // 체력바 이펙트
        player.HealthBar.TakeDamageEffect().Forget();
        // 피격 텍스트 띄우기
        HitTextUI hitText = ObjectPoolManager.Instance.Get(EPool.HitText, new Vector2(player.transform.position.x, player.transform.position.y + 1f), Quaternion.identity).GetComponent<HitTextUI>();
        hitText.InitializeHitText(dmg, EHitType.PlayerHit);

        Hp -= damage;
        HpChanged();
    }

    public async UniTaskVoid HPRegenRoutine()
    {
        // 1초마다 체력재생
        while (true)
        {
            Hp = Mathf.Clamp(Hp+1, 0, MaxHp);
            HpChanged();

            // 체력재생 공식 : 5 / (1 + (HpRegen - 1) / 2f) 초마다 1씩 재생
            await UniTask.Delay(hpRegenTimer, cancellationToken: player.DisableCancellation.Token);
        }
    }

    public void PlayerStatChanged(PlayerLevelUpData data)
    {
        switch (data.statType)
        {
            case EStatType.Hp:
                // HP 관련 처리
                MaxHp += data.value;
                HpChanged();
                break;
            case EStatType.HpRegen:
                // HP 재생 관련 처리
                HpRegen += data.value;
                hpRegenTimer = (int)(5 / (1 + (HpRegen - 1) / 2f) * 1000f);
                break;
            case EStatType.Defense:
                // 방어력 관련 처리
                Defense += data.value;
                def = UtilitieHelper.CombatScaling(Defense);
                break;
            case EStatType.BonusDamage:
                // 추가 데미지 관련 처리
                BonusDamage += data.value;
                break;
            case EStatType.MeleeDamage:
                // 근접 데미지 관련 처리
                MeleeDamage += data.value;
                break;
            case EStatType.RangeDamage:
                // 원거리 데미지 관련 처리
                RangeDamage += data.value;
                break;
            case EStatType.Speed:
                // 이동속도 관련 처리
                Speed = UtilitieHelper.IncreaseByPercent(Speed, data.value);
                break;
            case EStatType.Dodge:
                // 회피 관련 처리
                Dodge = Mathf.Clamp(Dodge+data.value, 0 ,80); // 최대 80%
                break;
            case EStatType.PickUpRange:
                // 획득 범위 관련 처리
                PickUpRange += data.value;
                player.CircleRange.radius = UtilitieHelper.IncreaseByPercent(player.CircleRange.radius, data.value);
                break;
            case EStatType.ExpBonus:
                // 경험치 보너스 관련 처리
                ExpBonus += data.value;
                break;
            default:
                break;
        }


        Debug.Log($"{MaxHp} , {HpRegen} , {Defense} , {BonusDamage} , {MeleeDamage} , {RangeDamage} , {Speed} , {Dodge}  , {PickUpRange} ,  {ExpBonus}");
    } 

    public void HpRecovery(int value)
    {
        float addHp = MaxHp * value * 0.01f;
        Hp = Mathf.Clamp(Hp + addHp, 0, MaxHp);
        HpChanged();
    }

    public float GetHPStatRatio()
        => Hp / MaxHp;


    private void HpChanged()
    {
        player.HealthBar.SetHealthBar(Hp / MaxHp);
        OnHpChanged?.Invoke(this, Hp);
    }

    //private void ExpChanged(int currentExp)
    //{
    //    if (currentExp >= Exp)
    //    {
    //        Level.Value++;
    //        CurrentExp.Value -= Exp;
    //        Exp = Exp + (int)(Exp * Settings.expPerLevel);

    //        // 레벨업시 체력 1 상승
    //        MaxHp++;
    //        Hp++;
    //        player.HealthBar.SetHealthBar(Hp / MaxHp);
    //    }    
 
    //}

    public void AddExp(int exp)
    {
        CurrentExp.Value += exp;
    }

    public void LevelUp()
    {
        Level.Value++;
        CurrentExp.Value -= Exp;
        Exp += (int)(Exp * Settings.expPerLevel); // 레벨업 시 필요 경험치 증가

        MaxHp++;
        Hp++;
        player.HealthBar.SetHealthBar(Hp / MaxHp);
    }

}
