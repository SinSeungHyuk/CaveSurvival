using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MovementSpeedUp", menuName = "Scriptable Objects/Monster/MonsterMovement/SpeedUp")]
public class MovementSpeedUp : MonsterMovementSO
{
    private bool isMove = false;


    public override void Move()
    {
        base.Move(); // 플레이어와의 거리 구하기

        if (!isMove)
            SpeedUp().Forget();

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

    private async UniTask SpeedUp()
    {
        isMove = true;

        while (true)
        {
            monster.Stat.Speed = Mathf.Clamp(UtilitieHelper.IncreaseByPercent(monster.Stat.Speed, 50), 1, 10);

            await UniTask.Delay(4000, cancellationToken: monster.DisableCancellation.Token);
        }
    }
} 