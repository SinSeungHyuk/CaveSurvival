using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "DetectorLowHealth", menuName = "Scriptable Objects/Weapon/Detectors/LowHealth")]
public class DetectorLowHealth : WeaponDetectorSO
{
    public override bool DetectMonster(Weapon weapon, out Vector2 direction, out Monster monster)
    {
        var playerPosition = weapon.Player.transform.position;
        var colliders = Physics2D.OverlapCircleAll(playerPosition, weapon.WeaponRange, Settings.monsterLayer);
        var monsters = colliders
            .Where(collider => !collider.GetComponent<Monster>().IsDead)
            .Select(collider => collider.GetComponent<Monster>())
            .ToList();

        if (monsters.Count == 0)
        {
            direction = Vector2.zero;
            monster = null;
            return false;
        }

        monster = null;
        float lowHp = Mathf.Infinity;
        for (int i = 0; i < monsters.Count; i++)
        {
            // 가장 낮은 체력과 현재 몬스터의 체력 비교
            float hp = monsters[i].Stat.Hp;
            if (hp < lowHp)
            {
                monster = monsters[i];
                lowHp = hp;
            }
        }

        direction = (monster.transform.position - playerPosition).normalized;
        return true;
    }
}
