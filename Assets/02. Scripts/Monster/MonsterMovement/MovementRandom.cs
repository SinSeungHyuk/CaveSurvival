using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MovementRandom", menuName = "Scriptable Objects/Monster/MonsterMovement/Random")]
public class MovementRandom : MonsterMovementSO
{
    private bool isMove = false;

    public override void Move()
    {
        base.Move();

        if (!isMove)
            SetRandomDir().Forget();
    }

    private async UniTask SetRandomDir()
    {
        isMove = true;

        rigid.mass = 1f;

        while (true)
        {
            float x = Random.Range(-1.0f, 1.0f);
            float y = Random.Range(-1.0f, 1.0f);

            moveVec.x = x;
            moveVec.y = y;
            moveVec = moveVec.normalized;

            rigid.velocity = moveVec * monster.Stat.Speed;

            await UniTask.Delay(2000, cancellationToken: monster.DisableCancellation.Token);
        }
    }
}