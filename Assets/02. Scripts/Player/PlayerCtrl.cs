using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour, IMovement, IAttack
{
    private Player player;
    private FloatingJoystick joy;
    private Rigidbody2D rigid;
    private Vector2 moveVec;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();

        joy = GameObject.FindWithTag("GameController").GetComponent<FloatingJoystick>();
        StageManager.Instance.OnWaveFinished += Instance_OnWaveFinished;
    }
    
    private void Update()
    {
        Attack();
    }

    private void FixedUpdate()
    {
        Move();
    }


    private void Instance_OnWaveFinished()
    => transform.position = Vector2.zero;

    #region INTERFACE
    public void Attack()
    {
        for (int i = 0; i < player.WeaponList.Count; ++i) // 플레이어의 무기 리스트 각각 공격이벤트 호출
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