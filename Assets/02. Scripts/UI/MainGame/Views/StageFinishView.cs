using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageFinishView : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private PlayerStatView playerStatView;
    [SerializeField] private TextMeshProUGUI txtPlayerTotalKills;

    [Header("Weapon Stats")]
    [SerializeField] private List<WeaponTotalStatsUI> weaponStatsUI;


    private void OnEnable()
    {
        Time.timeScale = 0f;
    }
    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void InitializeStageFinishView(PlayerStat stat, List<Weapon> weaponList)
    {
        playerStatView.InitializePlayerStatView(stat);

        txtPlayerTotalKills.text = GameStatsManager.Instance.GetStats(EStatsType.PlayerTotalKills).ToString();

        for (int i = 0; i < weaponList.Count; i++)
        {
            weaponStatsUI[i].InitializeWeaponTotalStatsUI(weaponList[i]);
        }

        gameObject.SetActive(true);
    }

    public void OnBtnExit()
    {
        // ����ȭ������ �� �̵�
        AddressableManager.Instance.ReleaseGroup("Stage1");
        LoadingSceneManager.LoadScene("MainMenuScene", "NULL", ESceneType.MainMenu);
    }
}
