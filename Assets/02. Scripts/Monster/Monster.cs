using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    private MonsterDetailsSO enemyDetails;
    private SpriteRenderer sprite;
    private MonsterMovementSO movement;
    private MonsterAttackSO monsterAttack;
    private MonsterStat stat;
    private HealthBarUI healthBar;
    private Rigidbody2D rigid;
    private PolygonCollider2D hitbox;
    private Animator animator;
    private SoundEffectSO hitSoundEffect;

    private bool isDead;
    private bool isBoss;

    #region MONSTER EVENT
    private MonsterDestroyedEvent monsterDestroyedEvent;
    #endregion

    public Player Player { get; private set; }
    public Transform PlayerTransform => Player.transform;
    public ItemDetailsSO DropItem { get; private set; }
    public MonsterState MonsterState { get; private set; }
    public MonsterDetailsSO EnemyDetails => enemyDetails;
    public MonsterStat Stat => stat;
    public SpriteRenderer Sprite => sprite;
    public Rigidbody2D Rigid => rigid;
    public bool IsDead => isDead;   
    public bool IsBoss => isBoss;

    // ���Ͱ� ��Ȱ��ȭ�Ǹ鼭 �̵�,���� �����½�ũ ����ؾ���
    public CancellationTokenSource DisableCancellation { get; private set; }



    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
        healthBar = GetComponent<HealthBarUI>();
        MonsterState = GetComponentInChildren<MonsterState>();
        isBoss = (healthBar != null) ? true : false; // ������ ü�¹ٸ� ������ ����
        monsterDestroyedEvent = GetComponent<MonsterDestroyedEvent>();
        stat = new MonsterStat();
    }
    private void Start()
    {
        hitSoundEffect = AddressableManager.Instance.GetResource<SoundEffectSO>("SoundEffect_Hit");
    }
    private void OnEnable()
    {
        // Ȱ��ȭ �Ǹ鼭 ��ū ���Ӱ� �ʱ�ȭ
        DisableCancellation = new CancellationTokenSource();
    }

    private void OnDisable()
    {
        // ��Ȱ��ȭ �Ǹ鼭 �����½�ũ ��Ҹ��
        DisableCancellation.Cancel();
        DisableCancellation.Dispose();
    }

    private void FixedUpdate()
    {
        movement?.Move();
    }


    public void InitializeMonster(MonsterDetailsSO data, int waveCount)
    {
        enemyDetails = data;

        Player = GameManager.Instance.Player;
        stat.InitializeMonsterStat(enemyDetails, waveCount); // ���� ���̺꿡 ���߾� �����ʱ�ȭ

        animator.runtimeAnimatorController = data.runtimeAnimatorController;
        sprite.sprite = enemyDetails.sprite;
        sprite.color = Color.white;
        sprite.material = data.enemyStandardMaterial;
        rigid.freezeRotation = true;
        rigid.mass = (isBoss) ? 1f : 0.1f;
        transform.localScale = Vector3.one;
        hitbox.enabled = true;
        isDead = false;
        DropItem = enemyDetails.itemDetails;

        movement = enemyDetails.movementType.Clone() as MonsterMovementSO;
        monsterAttack = enemyDetails.attackType?.Clone() as MonsterAttackSO; // ����Ÿ���� �������� ����
        MonsterBehaviourInitialize();

        List<Vector2> spritePhysicsShapePointsList = new List<Vector2>();
        sprite.sprite.GetPhysicsShape(0, spritePhysicsShapePointsList); // ��������Ʈ �׵θ� ������
        hitbox.points = spritePhysicsShapePointsList.ToArray(); // �ǰ����� �浹ü �׸���

        // Ȱ��ȭ �Ǹ鼭 ���� ���������� ���̺� ���� �̺�Ʈ ����
        StageManager.Instance.CurrentStage.MonsterSpawnEvent.OnWaveFinish += Stage_OnWaveFinished;
    }

    public void Stage_OnWaveFinished(MonsterSpawnEvent @event)
    {
        // ��Ȱ��ȭ �Ǹ鼭 ���� ���������� ���̺� ���� �̺�Ʈ ��������
        StageManager.Instance.CurrentStage.MonsterSpawnEvent.OnWaveFinish -= Stage_OnWaveFinished;

        ObjectPoolManager.Instance.Release(gameObject, EPool.Monster);
    }

    public void TakeDamage(Weapon weapon, int bonusDamage = 0) // '����'�� ���� ���ظ� ������
    {
        SoundEffectManager.Instance.PlaySoundEffect(hitSoundEffect);

        int dmg = GetDamage(weapon, bonusDamage);
        stat.Hp -= dmg;

        // ���ظ� �� ������ ������ ��� ����
        GameStatsManager.Instance.AddStats(weapon.WeaponDetails, EStatsType.WeaponTotalDamage, dmg);

        healthBar?.SetHealthBar(stat.Hp / enemyDetails.maxHp); // ������ ü�¹ٸ� �����������Ƿ� null üũ

        if (IsMonsterDead())
        {
            Player.AddUltimateGauge((int)enemyDetails.itemDetails.itemGrade);
            return;
        }
        else
            TakeDamageEffect().Forget();
    }
    public void TakeDamage(int damage) // ���Ⱑ �ƴ� �ٸ��Ϳ� ���� ���ظ� ������
    {
        SoundEffectManager.Instance.PlaySoundEffect(hitSoundEffect);

        stat.Hp -= damage;
        healthBar?.SetHealthBar(stat.Hp / enemyDetails.maxHp); // ������ ü�¹ٸ� �����������Ƿ� null üũ

        var hitText = ObjectPoolManager.Instance.Get(EPool.HitText, new Vector2(transform.position.x, transform.position.y + 0.75f), Quaternion.identity);
        hitText.GetComponent<HitTextUI>().InitializeHitText(damage, EHitType.Critical);

        if (IsMonsterDead())
            return;
        else
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

            var hitText = ObjectPoolManager.Instance.Get(EPool.HitText, new Vector2(transform.position.x, transform.position.y + 0.75f), Quaternion.identity);
            hitText.GetComponent<HitTextUI>().InitializeHitText(damage, EHitType.Critical);
        }
        else
        {
            var hitText = ObjectPoolManager.Instance.Get(EPool.HitText, new Vector2(transform.position.x, transform.position.y + 0.75f), Quaternion.identity);
            hitText.GetComponent<HitTextUI>().InitializeHitText(damage);
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

    private void MonsterBehaviourInitialize()
    {
        movement.InitializeMonsterMovement(this);
        monsterAttack?.InitializeMonsterAttack(this);
        monsterAttack?.Attack(); // ���� Ȱ��ȭ�Ǹ鼭 ���ݽ���
    }

    private bool IsMonsterDead()
    {
        if (stat.Hp <= 0f) // ���ʷ� ü���� 0 ���Ϸ� ������ ���
        {
            isDead = true;

            PostMonsterDead();

            return true;
        }

        return false;
    }

    private void PostMonsterDead() // ���Ͱ� �װ��� �ؾߵǴ� �۾���
    {
        // Clone�� SO �ν��Ͻ� ����
        Destroy(movement); // Clone�� SO �ı�
        Destroy(monsterAttack); // Clone�� SO �ı�
        movement = null;
        monsterAttack = null;

        MonsterState.ClearAllDebuff(); // ������ ��� ����� ����

        hitbox.enabled = false; // �浹ü ����

        // ����̺�Ʈ ó��
        monsterDestroyedEvent.CallMonsterDestroyedEvent(this.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾�� �ε����� ������ �ֱ�
        if (collision.TryGetComponent(out Player player))
        {
            player.TakeDamage(stat.Atk);
        }  
    }
}
