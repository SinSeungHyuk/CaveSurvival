using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponTotalStatsUI : MonoBehaviour
{
    [SerializeField] private Image weaponSprite;
    [SerializeField] private TextMeshProUGUI txtWeaponTotalDamage;
    [SerializeField] private TextMeshProUGUI txtWeaponDPS;

    private int totalDamage;


    public void InitializeWeaponTotalStatsUI(Weapon weapon)
    {
        totalDamage = GameStatsManager.Instance.GetStats(weapon.WeaponDetails, EStatsType.WeaponTotalDamage);
        int dps = GetWeaponDPS(weapon);

        weaponSprite.sprite = weapon.WeaponSprite;
        txtWeaponTotalDamage.text = totalDamage.ToString("N0");
        txtWeaponDPS.text = dps.ToString("N0");
    }

    private int GetWeaponDPS(Weapon weapon)
    {
        int startTime = GameStatsManager.Instance.GetStats(weapon.WeaponDetails, EStatsType.WeaponAcquiredTime);
        int startWave = GameStatsManager.Instance.GetStats(weapon.WeaponDetails, EStatsType.WeaponAcquiredWave);

        int endWave = StageManager.Instance.CurrentStage.MonsterSpawner.WaveCount;
        int endTime = (int)StageManager.Instance.CurrentStage.MonsterSpawner.ElapsedTime;

        // ���⸦ ���� ���忡�� ���������� ������ ��� �ð����̸� ���
        if (startWave == endWave)
            return totalDamage / (endTime - startTime);


        // 2���̺� 13�� ����� ���� ȹ��� : 2���̺�(25��) - 13�� = 12�� ȹ��
        int startElapsedTime = UtilitieHelper.GetWaveTimer(startWave) - startTime;

        int middleTime = 0; // ���⸦ ���� ���̺�� ������ ���̺� ������ ���̺�ð� ����
        for (int i = startWave + 1; i < endWave; i++)
        {
            middleTime += UtilitieHelper.GetWaveTimer(i);
        }

        return totalDamage / (middleTime + startElapsedTime + endTime);
    }
}