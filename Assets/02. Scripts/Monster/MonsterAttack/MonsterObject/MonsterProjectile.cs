using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterProjectile : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private float speed;
    private float dmg;

    private float currentDistance;
    private Vector2 direction;
    private Vector2 distanceVector;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void InitializeMonsterProjectile(float speed, Vector2 moveVec, float dmg)
    {
        this.speed = speed;
        this.direction = moveVec; // ����ü ���⺤��
        this.dmg = dmg;
        currentDistance = 0f;
    }


    private void FixedUpdate()
    {
        // FixedUpdate���� ������ �������� ������ ���� speed��ŭ �̵�
        distanceVector = direction * speed;
        currentDistance += distanceVector.magnitude * Time.fixedDeltaTime; // ���ư� �Ÿ������ ���� magnitude
        rigidBody.velocity = distanceVector;

        if (currentDistance > Settings.monsterProjectileDist)
            ObjectPoolManager.Instance.Release(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            player.TakeDamage(dmg);
        }

        ObjectPoolManager.Instance.Release(this.gameObject);
    }
}
