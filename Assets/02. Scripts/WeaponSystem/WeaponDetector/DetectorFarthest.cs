using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DetectorFarthest", menuName = "Scriptable Objects/Weapon/Detectors/Farthest")]
public class DetectorFarthest : WeaponDetectorSO
{
    public override bool DetectMonster(Weapon weapon, out Vector2 direction, out GameObject monster)
    {
        var playerPosition = weapon.Player.transform.position;
        var colliders = Physics2D.OverlapCircleAll(playerPosition, weapon.WeaponRange, Settings.monsterLayer);

        if (colliders.Length == 0)
        {
            direction = Vector2.zero;
            monster = null;
            return false;
        }

        monster = null;
        float dist = 0;
        for (int i = 0; i < colliders.Length; i++)
        {
            // �ܼ� �Ÿ����̹Ƿ� ��귮�� ���� sqrMagnitude ���
            float distance = (colliders[i].transform.position - playerPosition).sqrMagnitude;
            if (dist < distance)
            {
                // ���̾� �˻縦 ���� ������ �����̹Ƿ� GetComponent ����
                monster = colliders[i].gameObject;
                dist = distance;
            }
        }

        direction = (monster.transform.position - playerPosition).normalized;
        return true;
    }
}
