using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MovementBase", menuName = "Scriptable Objects/Monster/MonsterMovement/Base")]
public class MovementBase : MonsterMovementSO
{
    public override void Move()
    {
        base.Move(); // 플레이어와의 거리 구하기

        if ((int)dist == (int)monster.Stat.ChaseDistance) 
        {
            // 추적 거리에 도달시 멈추기
            rigid.velocity = Vector2.zero;
            return;
        }
        // 플레이어와의 간격 유지하기
        else if (dist > monster.Stat.ChaseDistance)
            moveVec = (monster.PlayerTransform.position - monster.transform.position).normalized;
        else
            moveVec = (monster.transform.position - monster.PlayerTransform.position).normalized;

        rigid.velocity = moveVec * monster.Stat.Speed;
    }
}
