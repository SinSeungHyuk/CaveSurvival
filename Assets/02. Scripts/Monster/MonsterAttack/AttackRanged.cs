using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AttackRanged", menuName = "Scriptable Objects/Monster/MonsterAttack/Ranged")]
public class AttackRanged : MonsterAttackSO
{
    [SerializeField] private float speed = 10f; // 투사체 속도
    private Vector2 fireDir;
    private GameObject projectileObject;


    public override void Attack()
       => ProjectileLaunch().Forget();


    private async UniTask ProjectileLaunch()
    {
        while (true)
        {
            // 몬스터가 중간에 비활성화될때를 대비
            await UniTask.Delay(Settings.monsterFireRate, cancellationToken: monster.DisableCancellation.Token);

            fireDir = (monster.Player.position - monster.transform.position).normalized;

            // 발사 명령이 떨어지면 풀에서 투사체 활성화
            projectileObject = ObjectPoolManager.Instance.Get(EPool.MonsterBullet, monster.transform);
            // 속도, 방향, 데미지 넣어서 초기화
            projectileObject.GetComponent<MonsterProjectile>().InitializeMonsterProjectile(speed, fireDir, monster.Stat.Atk);
        }
    }
}
