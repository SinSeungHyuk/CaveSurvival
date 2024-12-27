using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MovementCharge", menuName = "Scriptable Objects/Monster/MonsterMovement/Charge")]
public class MovementCharge : MonsterMovementSO
{
    [SerializeField] private float chargeSpeed = 10f; // �����ӵ�

    private bool isCharge; // ���� ������


    public override void Move()
    {
        base.Move(); // �÷��̾���� �Ÿ� ���ϱ�

        // ���� �Ÿ��� ���԰� �������� �ƴ϶�� ��������
        if (dist < monster.Stat.ChaseDistance && isCharge == false)
        {
            rigid.velocity = Vector2.zero;
            Charge().Forget();
            return;
        }

        // ���� �������� �ƴҶ��� �ɾ �̵�
        else if (isCharge == false)
        {
            moveVec = (monster.Player.position - monster.transform.position).normalized;
            rigid.velocity = moveVec * monster.Stat.Speed;
        }
    }

    private async UniTask Charge()
    {
        isCharge = true;
        rigid.mass = 100; // ���� �غ����϶� �˹� X

        monster.Sprite.color = Settings.legend; // ���� �غ����϶� �� ��ȭ

        await UniTask.Delay(1000);

        monster.Sprite.color = Color.white;

        moveVec = (monster.Player.position - monster.transform.position).normalized;
        rigid.velocity = moveVec * chargeSpeed;

        await UniTask.Delay(1000);

        isCharge = false;
        rigid.mass = 0.1f;

        return;
    }
}
