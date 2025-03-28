using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DetectorNearest", menuName = "Scriptable Objects/Weapon/Detectors/Nearest")]
public class DetectorNearest : WeaponDetectorSO
{
    public override bool DetectMonster(Weapon weapon, out Vector2 direction, out Monster monster)
    {
        var playerPosition = weapon.Player.transform.position;
        var colliders = Physics2D.OverlapCircleAll(playerPosition, weapon.WeaponRange, Settings.monsterLayer);
        var monsters = colliders
            .Where(collider => !collider.GetComponent<Monster>().IsDead)
            .Select(collider => collider.GetComponent<Monster>())
            .ToList();

        if (colliders.Length == 0)
        {
            direction = Vector2.zero;
            monster = null;
            return false;
        }

        monster = null;
        float dist = Mathf.Infinity;
        for (int i = 0; i < monsters.Count; i++)
        {
            // 단순 거리비교이므로 계산량이 적은 sqrMagnitude 사용
            float distance = (colliders[i].transform.position - playerPosition).sqrMagnitude;
            if (dist > distance)
            {
                // 레이어 검사를 통해 무조건 몬스터이므로 GetComponent 생략
                monster = monsters[i];
                dist = distance;
            }
        }

        direction = (monster.transform.position - playerPosition).normalized;
        return true;
    }
}
