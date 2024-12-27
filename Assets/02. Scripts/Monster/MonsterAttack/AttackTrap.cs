using Cysharp.Threading.Tasks;
using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AttackTrap", menuName = "Scriptable Objects/Monster/MonsterAttack/Trap")]
public class AttackTrap : MonsterAttackSO
{
    [SerializeField] private int count = 5; // ���� ����
    private GameObject trap;
    private Vector2 trapPos;


    public override void Attack()
       => CreateTrapRoutine().Forget();


    private async UniTask CreateTrapRoutine()
    {
        while (true)
        {
            for (int i = 0; i < count; i++)
            {
                // �÷��̾� ��ġ ���� x,y +-5 ��ġ�� ���� ����
                float dx = Random.Range(-5.0f, 5.0f);
                float dy = Random.Range(-5.0f, 5.0f);
                trapPos.x = monster.Player.position.x + dx;
                trapPos.y = monster.Player.position.y + dy;
                CreateTrap();
            }

            await UniTask.Delay(Settings.monsterFireRate, cancellationToken: monster.DisableCancellation.Token);
            // ���Ͱ� �߰��� ��Ȱ��ȭ�ɶ��� ���
        }
    }

    private void CreateTrap()
    {
        // �������� ������ ��ġ�� Ʈ�� Ȱ��ȭ
        trap = ObjectPoolManager.Instance.Get("MonsterTrap", trapPos, Quaternion.identity);
        // ������ �־ �ʱ�ȭ
        trap.GetComponent<MonsterTrap>().InitializeMonsterTrap(monster.Stat.Atk);
    }
}
