using ExitGames.Client.Photon.StructWrapping;
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
    // private int piercingCount; // ���� ī��Ʈ
    //private SoundEffectSO soundEffect;

    private float distance;
    private float currentDistance;
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
            ObjectPoolManager.Instance.Release(this.gameObject);
    }

    public void InitializeProjectile(ProjectileDetailsSO projectileDetails, Vector2 direction, Weapon weapon)
    {
        spriteRenderer.sprite = projectileDetails.sprite;

        //soundEffect = weapon.weaponFiringSoundEffect;
        this.projectileEffect = projectileDetails.projectileEffect;
        projectileEffect.InitializePE(projectileDetails.bonusEffects);
        this.speed = projectileDetails.projectileSpeed;
        this.isPiercing = projectileDetails.isPiercing;
        this.direction = direction; // ����ü ���⺤��
        this.weapon = weapon;
        distance = weapon.WeaponRange;
        currentDistance = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Monster monster))
        {
            //SoundEffectManager.Instance.PlaySoundEffect(soundEffect);
            projectileEffect.Apply(monster, weapon, direction);
        }

        // ����Ǵ� ���� ���ϰ� ������ ����Ǵ� ���� ���� count�� ���� ��
        // ������ ����Ǵ��� ���ο� ���� ��� �����̰ų� �ȵǰų��� ����
        if (!isPiercing)
            ObjectPoolManager.Instance.Release(this.gameObject);
    }
}
