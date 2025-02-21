using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    private PlayerDetailsSO playerDetails; // ĳ������ ������ ���������� ���� �ʿ俩�� �޶���
    private SpriteRenderer spriteRenderer;

    private Animator animator;
    private PlayerStat stat; // ĳ���� ����
    private PlayerCtrl ctrl; // ĳ���� ��Ʈ�ѷ�


    #region PLAYER EVENT
    public WeaponAttackEvent WeaponAttackEvent { get; private set; }
    #endregion

    public SpriteRenderer SpriteRenderer => spriteRenderer;
    public PlayerStat Stat => stat;
    public CircleCollider2D CircleRange { get; private set; } // �ڼ�����
    public HealthBarUI HealthBar {  get; private set; }
    public List<Weapon> WeaponList { get; private set; } // ���� ����Ʈ
    public WeaponTransform WeaponTransform {  get; private set; } // ���� ���� Ʈ������
    public PlayerWaveBuff PlayerWaveBuff { get; private set; }
    public PlayerLevelUp PlayerLevelUp { get; private set; }
    public UltimateSkillBehaviour UltimateSkillBehaviour { get; private set; }
    public CancellationTokenSource DisableCancellation { get; private set; }


    #region TEST
    public WeaponDetailsSO weapon2;
    #endregion



    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        CircleRange = GetComponentInChildren<CircleCollider2D>();
        ctrl = GetComponent<PlayerCtrl>();
        WeaponTransform = GetComponentInChildren<WeaponTransform>();
        PlayerWaveBuff = GetComponentInChildren<PlayerWaveBuff>();
        PlayerLevelUp = GetComponent<PlayerLevelUp>();
        HealthBar = GetComponent<HealthBarUI>();
        stat = new PlayerStat();

        WeaponAttackEvent = GetComponent<WeaponAttackEvent>();
        DisableCancellation = new CancellationTokenSource();
    }

    private void OnEnable()
    {
        // Ȱ��ȭ �Ǹ鼭 ��ū ���Ӱ� �ʱ�ȭ
        DisableCancellation = new CancellationTokenSource();
    }
    private void OnDisable()
    {
        // ��Ȱ��ȭ �Ǹ鼭 ��Ҹ��
        DisableCancellation.Cancel();
        DisableCancellation.Dispose();
    }

    private void Start()
    {
        //AddWeaponTest();
        // ��Ÿ�� ������ DPS ��踦 ���� �ʱ�ȭ �������� �̸� �߰����ֱ�
        GameStatsManager.Instance.AddStats(playerDetails.playerStartingWeapon, EStatsType.WeaponAcquiredTime, 0);
        GameStatsManager.Instance.AddStats(playerDetails.playerStartingWeapon, EStatsType.WeaponAcquiredWave, 0);
    }


    public void InitializePlayer(PlayerDetailsSO so)
    {
        this.playerDetails = so;

        spriteRenderer.sprite = playerDetails.playerSprite; 
        animator.runtimeAnimatorController = playerDetails.runtimeAnimatorController;
        WeaponList = new List<Weapon>(Settings.maxWeaponCount);
        AddWeaponToPlayer(playerDetails.playerStartingWeapon);

        UltimateSkillBehaviour = so.playerStartingUltimateSkill.CreateSkill(this);

        stat.InitializePlayerStat(playerDetails, this);
    }

    public Weapon AddWeaponToPlayer(WeaponDetailsSO weaponDetails)
    {
        // �߰��� ���� �ʱ�ȭ
        Weapon playerWeapon = gameObject.AddComponent<Weapon>();
        playerWeapon.InitializeWeapon(weaponDetails);
        playerWeapon.Player = this;

        WeaponList.Add(playerWeapon); // ���� ����Ʈ�� �߰�
        WeaponTransform.Add(playerWeapon);



        return playerWeapon;
    }

    public void TakeDamage(float dmg)
    {
        // ���ȿ��� ü�� ���̴� �Լ� ���� (���,ȸ�� ���)
        stat.TakeDamage(dmg);

        if (stat.Hp <= 0f)
        {
            // ����̺�Ʈ ó��
            StageManager.Instance.CurrentStage.MonsterSpawnEvent.CallStageFinish();

            return;
        }
    }

    public void HpRecoveryPercent(int value)
    {
        // value % ��ŭ ���� ü�� ȸ��
        Stat.HpRecovery(value);
    }

    public void UseUltimateSkill()
    {
        UltimateSkillBehaviour.UseUltimateSkill();
    }

    public void AddUltimateGauge(int value)
    {
        UltimateSkillBehaviour.SetGaugeRatio(value);
    }


    #region TEST FUNCTION
    public void AddWeaponTest()
        => AddWeaponToPlayer(weapon2);
    #endregion
}
