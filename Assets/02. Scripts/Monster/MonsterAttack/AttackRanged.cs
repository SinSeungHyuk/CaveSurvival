using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AttackRanged", menuName = "Scriptable Objects/Monster/MonsterAttack/Ranged")]
public class AttackRanged : MonsterAttackSO
{
    [SerializeField] private float speed = 10f; // ����ü �ӵ�
    private Vector2 fireDir;
    private GameObject projectileObject;


    public override void Attack()
       => ProjectileLaunch().Forget();


    private async UniTask ProjectileLaunch()
    {
        while (true)
        {
            // ���Ͱ� �߰��� ��Ȱ��ȭ�ɶ��� ���
            await UniTask.Delay(Settings.monsterFireRate, cancellationToken: monster.DisableCancellation.Token);

            fireDir = (monster.Player.position - monster.transform.position).normalized;

            // �߻� ����� �������� Ǯ���� ����ü Ȱ��ȭ
            projectileObject = ObjectPoolManager.Instance.Get(EPool.MonsterBullet, monster.transform);
            // �ӵ�, ����, ������ �־ �ʱ�ȭ
            projectileObject.GetComponent<MonsterProjectile>().InitializeMonsterProjectile(speed, fireDir, monster.Stat.Atk);
        }
    }
}
