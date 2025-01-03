using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AttackBossRanged", menuName = "Scriptable Objects/Monster/MonsterAttack/BossRanged")]
public class AttackBossRanged : MonsterAttackSO
{
    [SerializeField] private float speed = 10f; // ����ü �ӵ�
    private Vector2 fireDir;
    private GameObject projectileObject;
    private int projectileCount;


    public override void Attack()
       => ProjectileLaunch().Forget();


    private async UniTask ProjectileLaunch()
    {
        while (true)
        {
            projectileCount = Random.Range(10, 15);

            for (int i = 1; i <= projectileCount; i++)
            {
                fireDir.x = Random.Range(-1.0f, 1.0f);
                fireDir.y = Random.Range(-1.0f, 1.0f);
                fireDir = fireDir.normalized;

                // �߻� ����� �������� Ǯ���� ����ü Ȱ��ȭ
                projectileObject = ObjectPoolManager.Instance.Get(EPool.MonsterBullet, monster.transform);
                // �ӵ�, ����, ������ �־ �ʱ�ȭ
                projectileObject.GetComponent<MonsterProjectile>().InitializeMonsterProjectile(speed, fireDir, monster.Stat.Atk);
            }

            await UniTask.Delay(Settings.monsterFireRate, cancellationToken: monster.DisableCancellation.Token);
            // ���Ͱ� �߰��� ��Ȱ��ȭ�ɶ��� ���
        }
    }
}
