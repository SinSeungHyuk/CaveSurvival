using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private ProjectileEffectSO projectileEffect;

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Weapon weapon;
    private float speed;
    private bool isPiercing;
    private int piercingCount; // ���� ī��Ʈ

    private float distance;
    private float currentDistance;
    private int currentPiercingCount;
    private Vector2 direction;
    private Vector2 distanceVector;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        // FixedUpdate���� ������ �������� ������ ���� speed��ŭ �̵�
        distanceVector = direction * speed;
        currentDistance += distanceVector.magnitude * Time.fixedDeltaTime; // ���ư� �Ÿ������ ���� magnitude
        rigidBody.velocity = distanceVector;

        if (currentDistance > distance)
            ObjectPoolManager.Instance.Release(this.gameObject,EPool.Projectile);
    }

    public void InitializeProjectile(ProjectileDetailsSO projectileDetails, List<BonusEffectSO> bonusEffects, Vector2 direction, Weapon weapon)
    {
        spriteRenderer.sprite = projectileDetails.sprite;
        spriteRenderer.material = projectileDetails.material;

        this.projectileEffect = projectileDetails.projectileEffect;
        projectileEffect.InitializePE(bonusEffects);
        this.speed = projectileDetails.projectileSpeed;
        this.isPiercing = projectileDetails.isPiercing;
        this.piercingCount = projectileDetails.piercingCount;
        currentPiercingCount = 0;
        this.direction = direction; // ����ü ���⺤��
        this.weapon = weapon;
        distance = weapon.WeaponRange;
        currentDistance = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Monster monster))
        {
            projectileEffect.Apply(monster, weapon, direction);
        }

        // ����Ǵ� ���� ���ϰ� ������ ����Ǵ� ���� ���� count�� ���� ��
        if (!isPiercing)
            ObjectPoolManager.Instance.Release(this.gameObject, EPool.Projectile);
        else
        {
            currentPiercingCount++;

            if (piercingCount < currentPiercingCount) // ����Ƚ�� �ʰ��� ź ��ȯ
                ObjectPoolManager.Instance.Release(this.gameObject, EPool.Projectile);
        }
    }
}
