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
    private int piercingCount; // 관통 카운트

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
        // FixedUpdate에서 물리적 연산으로 전방을 향해 speed만큼 이동
        distanceVector = direction * speed;
        currentDistance += distanceVector.magnitude * Time.fixedDeltaTime; // 날아간 거리계산을 위한 magnitude
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
        this.direction = direction; // 투사체 방향벡터
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

        // 관통되는 수를 정하고 싶으면 관통되는 적의 수를 count로 세면 됨
        if (!isPiercing)
            ObjectPoolManager.Instance.Release(this.gameObject, EPool.Projectile);
        else
        {
            currentPiercingCount++;

            if (piercingCount < currentPiercingCount) // 관통횟수 초과시 탄 반환
                ObjectPoolManager.Instance.Release(this.gameObject, EPool.Projectile);
        }
    }
}
