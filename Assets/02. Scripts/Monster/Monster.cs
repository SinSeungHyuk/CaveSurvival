using Cysharp.Threading.Tasks;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviourPun
{
    [SerializeField] private Database monsterDB;

    private MonsterDetailsSO enemyDetails;
    private SpriteRenderer sprite;
    private MonsterMovementSO movement;
    private MonsterAttackSO monsterAttack;
    private MonsterStat stat;
    private Rigidbody2D rigid;
    private PolygonCollider2D hitbox;

    #region MONSTER EVENT
    private MonsterDestroyedEvent monsterDestroyedEvent;
    #endregion

    public Transform Player { get; private set; }
    public int DropItem { get; private set; }
    public MonsterDetailsSO EnemyDetails => enemyDetails;
    public MonsterStat Stat => stat;
    public SpriteRenderer Sprite => sprite;
    public Rigidbody2D Rigid => rigid;

    // ���Ͱ� ��Ȱ��ȭ�Ǹ鼭 �̵�,���� �����½�ũ ����ؾ���
    public CancellationTokenSource DisableCancellation { get; private set; }



    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<PolygonCollider2D>();
        monsterDestroyedEvent = GetComponent<MonsterDestroyedEvent>();
        stat = new MonsterStat();
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        // Ȱ��ȭ �Ǹ鼭 ��ū ���Ӱ� �ʱ�ȭ
        DisableCancellation = new CancellationTokenSource();
    }

    private void OnDisable()
    {
    }

    private void FixedUpdate()
    {
        movement?.Move();
    }

    [PunRPC]
    public void InitializeMonster(int id, int waveCount)
    {
        MonsterDetailsSO enemyDetails = monsterDB.GetDataByID<MonsterDetailsSO>(id);

        Player = GameManager.Instance.Player.transform;
        this.enemyDetails = enemyDetails;
        stat.InitializeMonsterStat(enemyDetails, waveCount); // ���� ���̺꿡 ���߾� �����ʱ�ȭ

        sprite.sprite = enemyDetails.sprite;
        rigid.freezeRotation = true;
        transform.localScale = Vector3.one;
        hitbox.enabled = true;

        movement = enemyDetails.movementType.Clone() as MonsterMovementSO;
        movement.InitializeMonsterMovement(this);

        monsterAttack = enemyDetails.attackType?.Clone() as MonsterAttackSO; // ����Ÿ���� �������� ����
        monsterAttack?.InitializeMonsterAttack(this);
        monsterAttack?.Attack(); // ���� Ȱ��ȭ�Ǹ鼭 ���ݽ���

        DropItem = enemyDetails.itemDetails.ID;

        List<Vector2> spritePhysicsShapePointsList = new List<Vector2>();
        sprite.sprite.GetPhysicsShape(0, spritePhysicsShapePointsList); // ��������Ʈ �׵θ� ������
        hitbox.points = spritePhysicsShapePointsList.ToArray(); // �ǰ����� �浹ü �׸���
    }

    public void TakeDamage(Weapon weapon, int bonusDamage = 0)
    {
        stat.Hp -= GetDamage(weapon, bonusDamage);

        if (stat.Hp <= 0f)
        {
            // ��Ȱ��ȭ �Ǹ鼭 ��Ҹ��
            DisableCancellation.Cancel();
            DisableCancellation.Dispose();

            // Ǯ�� ���ư� �� Clone�� SO �ν��Ͻ� ����       
            Destroy(movement); // Clone�� SO �ı�
            Destroy(monsterAttack); // Clone�� SO �ı�
            movement = null;
            monsterAttack = null;

            hitbox.enabled = false;

            // ����̺�Ʈ ó��
            monsterDestroyedEvent.CallMonsterDestroyedEvent(this.transform.position);
            return;
        }

        TakeDamageEffect().Forget();
    }

    private int GetDamage(Weapon weapon, int bonusDamage)
    {
        // �÷��̾��� ��/���Ÿ� �������� �߰�������% ���
        int damage = UtilitieHelper.IncreaseByPercent(weapon.WeaponDamage + bonusDamage, weapon.Player.Stat.BonusDamage);

        // ġ��Ÿ ����
        if (UtilitieHelper.isSuccess(weapon.WeaponCriticChance))
        {
            damage = UtilitieHelper.IncreaseByPercent(damage, weapon.WeaponCriticDamage);

            var hitText = ObjectPoolManager.Instance.Get("HitText", new Vector2(transform.position.x, transform.position.y + 0.75f), Quaternion.identity);
            //hitText.InitializeHitText(damage, true);
            hitText.GetComponent<PhotonView>().RPC("InitializeHitText", RpcTarget.All, damage,true,false);
        }
        else
        {
            var hitText = ObjectPoolManager.Instance.Get("HitText", new Vector2(transform.position.x, transform.position.y + 0.75f), Quaternion.identity);
            hitText.GetComponent<PhotonView>().RPC("InitializeHitText", RpcTarget.All, damage,false,false);
            //hitText.InitializeHitText(damage);
        }

        return damage;
    }

    private async UniTaskVoid TakeDamageEffect()
    {
        try
        {
            // �ǰݽ� 0.1�� ���� ��� ���׸���� ����
            sprite.material = enemyDetails.enemyHitMaterial;

            await UniTask.Delay(100);

            sprite.material = enemyDetails.enemyStandardMaterial;
        }
        catch (Exception ex)
        {
            Debug.Log($"TakeDamageEffect - {ex.Message}");
        }
    }
}
