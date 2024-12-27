using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Player : MonoBehaviour
{
    [SerializeField] private Database playerDB;

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
        HealthBar = GetComponent<HealthBarUI>();
        stat = new PlayerStat();

        WeaponAttackEvent = GetComponent<WeaponAttackEvent>();
        //playerDetails = GameManager.Instance.playerSO;
        DisableCancellation = new CancellationTokenSource();
        //InitializePlayer(playerDetails);
    }

    private void OnEnable()
    {
        // Ȱ��ȭ �Ǹ鼭 ��ū ���Ӱ� �ʱ�ȭ
        
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
    }

    [PunRPC]
    public void InitializePlayer(int id)
    {
        this.playerDetails = playerDB.GetDataByID<PlayerDetailsSO>(id);

        spriteRenderer.sprite = playerDetails.playerSprite; 
        //animator.runtimeAnimatorController = playerDetails.runtimeAnimatorController;
        WeaponList = new List<Weapon>(Settings.maxWeaponCount);
        AddWeaponToPlayer(playerDetails.playerStartingWeapon);

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
            
            return;
        }
    }


    #region TEST FUNCTION
    public void AddWeaponTest()
        => AddWeaponToPlayer(weapon2);
    #endregion
}
