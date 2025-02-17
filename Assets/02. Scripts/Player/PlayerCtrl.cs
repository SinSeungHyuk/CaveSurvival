using R3;
using R3.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour, IMovement, IAttack
{
    private Player player;
    private PlayerAnimator playerAnimator;
    private FloatingJoystick joy;
    private Rigidbody2D rigid;
    private Vector2 moveVec;
    private float speed;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        playerAnimator = GetComponent<PlayerAnimator>();

        joy = GameObject.FindWithTag("GameController").GetComponent<FloatingJoystick>();
    }
    private void OnEnable()
    {
        // Ȱ��ȭ �Ǹ鼭 ���� ���������� ���̺� ���� �̺�Ʈ ����
        StageManager.Instance.CurrentStage.MonsterSpawnEvent.OnWaveStart += Stage_OnWaveStart;
    }
    private void OnDisable()
    {
        // ��Ȱ��ȭ �Ǹ鼭 ���� ���������� ���̺� ���� �̺�Ʈ ��������
        StageManager.Instance.CurrentStage.MonsterSpawnEvent.OnWaveStart -= Stage_OnWaveStart;
    }

    private void Start()
    {
        this.UpdateAsObservable().Subscribe(_ => Move());
        this.UpdateAsObservable().Subscribe(_ => Attack());
    }

    private void Update()
    {
        //Attack();
    }

    private void FixedUpdate()
    {
        //Move();
    }


    private void Stage_OnWaveStart(MonsterSpawnEvent @event)
    => transform.position = Vector2.zero;

    #region INTERFACE
    public void Attack()
    {
        for (int i = 0; i < player.WeaponList.Count; ++i) // �÷��̾��� ���� ����Ʈ ���� �����̺�Ʈ ȣ��
        {
            player.WeaponAttackEvent.CallWeaponAttackEvent(player.WeaponList[i], i);
        }
    }

    public void Move()
    {
        speed = player.Stat.Speed;
        moveVec = joy.Direction * speed;
        playerAnimator.UpdateSpeed(moveVec.sqrMagnitude); // ���̽�ƽ ���Ͱ� 0���� ū���� �Ǵ�

        rigid.velocity = moveVec;

        if (moveVec.x < 0) 
            player.SpriteRenderer.flipX = true;
        else if (moveVec.x > 0)
            player.SpriteRenderer.flipX = false;
    }
    #endregion
}