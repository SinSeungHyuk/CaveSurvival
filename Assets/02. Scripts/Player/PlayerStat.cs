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
    public int Def => def; // ���� �氨�� %
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

        MaxHp = playerDetailsSO.Hp;
        Hp = MaxHp;
        HpRegen = playerDetailsSO.HpRegen;
        hpRegenTimer = (int)(5 / (1 + (HpRegen - 1) / 2f) * 1000f);
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
        // ȸ�� �˻�
        if (UtilitieHelper.isSuccess(Dodge))
        {
            HitTextUI dodgeText = ObjectPoolManager.Instance.Get(EPool.HitText, new Vector2(player.transform.position.x, player.transform.position.y + 1f), Quaternion.identity).GetComponent<HitTextUI>();
            dodgeText.InitializeHitText(0,EHitType.Dodge);

            return;
        }

        int dmg = ((int)UtilitieHelper.DecreaseByPercent(damage, def));

        // ü�¹� ����Ʈ
        player.HealthBar.TakeDamageEffect().Forget();
        // �ǰ� �ؽ�Ʈ ����
        HitTextUI hitText = ObjectPoolManager.Instance.Get(EPool.HitText, new Vector2(player.transform.position.x, player.transform.position.y + 1f), Quaternion.identity).GetComponent<HitTextUI>();
        hitText.InitializeHitText(dmg, EHitType.PlayerHit);

        Hp -= damage;
        HpChanged();
    }

    public async UniTaskVoid HPRegenRoutine()
    {
        // 1�ʸ��� ü�����
        while (true)
        {
            Hp = Mathf.Clamp(Hp+1, 0, MaxHp);
            HpChanged();

            // ü����� ���� : 5 / (1 + (HpRegen - 1) / 2f) �ʸ��� 1�� ���
            await UniTask.Delay(hpRegenTimer, cancellationToken: player.DisableCancellation.Token);
        }
    }

    public void PlayerStatChanged(EStatType statType, int value)
    {
        switch (statType)
        {
            case EStatType.Hp:
                // HP ���� ó��
                MaxHp += value;
                HpChanged();
                break;
            case EStatType.HpRegen:
                // HP ��� ���� ó��
                HpRegen += value;
                hpRegenTimer = (int)(5 / (1 + (HpRegen - 1) / 2f) * 1000f);
                break;
            case EStatType.Defense:
                // ���� ���� ó��
                Defense += value;
                def = UtilitieHelper.CombatScaling(Defense);
                break;
            case EStatType.BonusDamage:
                // �߰� ������ ���� ó��
                BonusDamage += value;
                break;
            case EStatType.MeleeDamage:
                // ���� ������ ���� ó��
                MeleeDamage += value;
                break;
            case EStatType.RangeDamage:
                // ���Ÿ� ������ ���� ó��
                RangeDamage += value;
                break;
            case EStatType.Speed:
                // �̵��ӵ� ���� ó��
                Speed = UtilitieHelper.IncreaseByPercent(Speed, value);
                break;
            case EStatType.Dodge:
                // ȸ�� ���� ó��
                Dodge = Mathf.Clamp(Dodge+value, 0 ,80); // �ִ� 80%
                break;
            case EStatType.PickUpRange:
                // ȹ�� ���� ���� ó��
                PickUpRange += value;
                player.CircleRange.radius = UtilitieHelper.IncreaseByPercent(player.CircleRange.radius, value);
                break;
            case EStatType.ExpBonus:
                // ����ġ ���ʽ� ���� ó��
                ExpBonus += value;
                break;
            case EStatType.SpeedBonus:
                // �̵��ӵ� ���ʽ� ���� ó��
                Speed += value;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
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

    public void AddExp(int exp)
    {
        CurrentExp.Value += (exp + ExpBonus);
    }

    public void LevelUp()
    {
        Level.Value++;
        CurrentExp.Value -= Exp;
        Exp += (int)(Exp * Settings.expPerLevel); // ������ �� �ʿ� ����ġ ����

        MaxHp++;
        Hp++;
        player.HealthBar.SetHealthBar(Hp / MaxHp);
    }

}
