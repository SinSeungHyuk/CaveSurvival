using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour, IMovement, IAttack
{
    private Player player;
    private FixedJoystick joy;
    private Rigidbody2D rigid;
    private Vector2 moveVec;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();

        joy = GameObject.FindWithTag("GameController").GetComponent<FixedJoystick>();
    }
    

    private void Update()
    {
        Attack();
    }

    private void FixedUpdate()
    {
        Move();
    }



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
        moveVec = joy.Direction * player.Stat.Speed;

        rigid.velocity = moveVec;
    }
    #endregion
}