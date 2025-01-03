using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat
{
    public event Action<PlayerStat, float> OnExpChanged;
    public event Action<PlayerStat, int> OnLevelChanged;


    private Player player;
    private int hpRegenTimer;


    #region LEVEL & EXP
    private int currentExp;

    public int Level { get; private set; }
    public int Exp { get; private set; }
    public int CurrentExp
    {
        get => currentExp;
        set
        {
            currentExp = value;

            if (currentExp >= Exp)
            {
                Level++;
                OnLevelChanged?.Invoke(this, Level);
                currentExp -= Exp;
                Exp = Exp + (int)(Exp * Settings.expPerLevel);
            }

            OnExpChanged?.Invoke(this, (float)currentExp / (float)Exp);
        }
    }
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
    public float PickUpRange { get; private set; }
    public int ExpBonus {  get; private set; }
    #endregion


    public void InitializePlayerStat(PlayerDetailsSO playerDetailsSO, Player player)
    {
        this.player = player;

        Level = 1;
        Exp = Settings.startExp;
        CurrentExp = 0;

        MaxHp = playerDetailsSO.Hp;
        Hp = MaxHp;
        HpRegen = playerDetailsSO.HpRegen;
        hpRegenTimer = (int)(5 / (1 + (HpRegen - 1) / 2.25f) * 1000f);
        //HpSteal = playerDetailsSO.HpSteal;
        Defense = playerDetailsSO.Defense;
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
            HitTextUI hitText = ObjectPoolManager.Instance.Get(EPool.HitText, new Vector2(player.transform.position.x, player.transform.position.y + 1f), Quaternion.identity).GetComponent<HitTextUI>();
            hitText.InitializeHitText(0,false,true);

            return;
        }

        int def = UtilitieHelper.CombatScaling(Defense);
        damage = UtilitieHelper.DecreaseByPercent(damage, def);

        Hp -= damage;
        player.HealthBar.SetHealthBar(Hp / MaxHp);
    }

    public async UniTaskVoid HPRegenRoutine()
    {
        // 1초마다 체력재생
        while (true)
        {
            Hp = Mathf.Clamp(Hp+1, 0, MaxHp);
            player.HealthBar.SetHealthBar(Hp / MaxHp);

            // 체력재생 공식 : 5 / (1 + (HpRegen - 1) / 2.25f) 초마다 1씩 재생
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
                break;
            case EStatType.HpRegen:
                // HP 재생 관련 처리
                HpRegen += data.value;
                hpRegenTimer = (int)(5 / (1 + (HpRegen - 1) / 2.25f) * 1000f);
                break;
            case EStatType.Defense:
                // 방어력 관련 처리
                Defense += data.value;
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
}
