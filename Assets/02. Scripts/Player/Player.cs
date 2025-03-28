using Cysharp.Threading.Tasks;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    private PlayerDetailsSO playerDetails; // 캐릭터의 종류가 나뉘는지에 따라 필요여부 달라짐
    private SpriteRenderer spriteRenderer;

    private Animator animator;
    private PlayerStat stat; // 캐릭터 스탯
    private PlayerCtrl ctrl; // 캐릭터 컨트롤러


    #region PLAYER EVENT
    public WeaponAttackEvent WeaponAttackEvent { get; private set; }
    #endregion

    public SpriteRenderer SpriteRenderer => spriteRenderer;
    public PlayerStat Stat => stat;
    public CapsuleCollider2D Hitbox {  get; private set; } // 히트박스
    public CircleCollider2D CircleRange { get; private set; } // 자석범위
    public HealthBarUI HealthBar {  get; private set; }
    public List<Weapon> WeaponList { get; private set; } // 무기 리스트
    public WeaponTransform WeaponTransform {  get; private set; } // 무기 장착 트랜스폼
    public PlayerWaveBuff PlayerWaveBuff { get; private set; }
    public PlayerLevelUp PlayerLevelUp { get; private set; }
    public PlayerSynergy PlayerSynergy { get; private set; }
    public UltimateSkillBehaviour UltimateSkillBehaviour { get; private set; }
    public CancellationTokenSource DisableCancellation { get; private set; }



    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Hitbox = GetComponent<CapsuleCollider2D>();
        CircleRange = GetComponentInChildren<CircleCollider2D>();
        ctrl = GetComponent<PlayerCtrl>();
        WeaponTransform = GetComponentInChildren<WeaponTransform>();
        PlayerWaveBuff = GetComponentInChildren<PlayerWaveBuff>();
        PlayerLevelUp = GetComponent<PlayerLevelUp>();
        PlayerSynergy = GetComponent<PlayerSynergy>();
        HealthBar = GetComponent<HealthBarUI>();
        stat = new PlayerStat();

        WeaponAttackEvent = GetComponent<WeaponAttackEvent>();
        DisableCancellation = new CancellationTokenSource();
    }

    private void OnEnable()
    {
        // 활성화 되면서 토큰 새롭게 초기화
        DisableCancellation = new CancellationTokenSource();
    }
    private void OnDisable()
    {
        // 비활성화 되면서 취소명령
        DisableCancellation.Cancel();
        DisableCancellation.Dispose();
    }

    private void Start()
    {
        // 스타팅 무기의 DPS 통계를 위해 초기화 과정에서 미리 추가해주기
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
        // 추가할 무기 초기화
        Weapon playerWeapon = gameObject.AddComponent<Weapon>();
        playerWeapon.InitializeWeapon(weaponDetails);
        playerWeapon.Player = this;

        WeaponList.Add(playerWeapon); // 무기 리스트에 추가
        WeaponTransform.Add(playerWeapon);
        PlayerSynergy.AddWeaponToPlayer(weaponDetails); // 무기 시너지 추가

        return playerWeapon;
    }

    public void TakeDamage(float dmg)
    {
        // 스탯에서 체력 깎이는 함수 구현 (방어,회피 계산)
        stat.TakeDamage(dmg);

        if (stat.Hp <= 0f)
        {
            Hitbox.enabled = false;
            // 사망이벤트 처리
            StageManager.Instance.CurrentStage.MonsterSpawnEvent.CallStageFinish();

            return;
        }
    }

    public void HpRecoveryPercent(int value)
    {
        // value % 만큼 현재 체력 회복
        Stat.HpRecovery(value);
    }

    public void UseUltimateSkill()
    {
        UltimateSkillBehaviour.UseUltimateSkill().Forget();
    }
    public void AddUltimateGauge(int value)
    {
        UltimateSkillBehaviour.SetGaugeRatio(value);
    }
}
