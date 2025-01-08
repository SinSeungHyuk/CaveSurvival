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

        // 무기를 얻은 라운드에서 스테이지가 끝났을 경우 시간차이만 계산
        if (startWave == endWave)
            return totalDamage / (endTime - startTime);


        // 2웨이브 13초 경과때 무기 획득시 : 2웨이브(25초) - 13초 = 12초 획득
        int startElapsedTime = UtilitieHelper.GetWaveTimer(startWave) - startTime;

        int middleTime = 0; // 무기를 얻은 웨이브와 마지막 웨이브 사이의 웨이브시간 누적
        for (int i = startWave + 1; i < endWave; i++)
        {
            middleTime += UtilitieHelper.GetWaveTimer(i);
        }

        return totalDamage / (middleTime + startElapsedTime + endTime);
    }
}