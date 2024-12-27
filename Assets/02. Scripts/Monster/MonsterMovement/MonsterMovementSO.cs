using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class MonsterMovementSO : ScriptableObject,IMovement , ICloneable
{
    protected Rigidbody2D rigid;
    protected Monster monster;
    protected float dist;
    protected Vector2 moveVec;


    public void InitializeMonsterMovement(Monster monster)
    {
        this.monster = monster;
        rigid = monster.Rigid;
    }


    #region INTERFACE
    public virtual void Move()
    {
        // ��� �̵����Ͽ��� �÷��̾���� �Ÿ��� ������ �ʿ��� -> �ڵ� �ߺ����� �ʰ� ���⼭ ���ϱ�
        dist = (monster.Player.position - monster.transform.position).sqrMagnitude;
    }

    public object Clone()
        => Instantiate(this);
    #endregion
}
