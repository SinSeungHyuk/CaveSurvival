using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour
{
    [Header("Player Stat")]
    [SerializeField] private PlayerStatView playerStatView;

    [Header("Weapon Stat")]
    [SerializeField] private List<WeaponStatUI> weaponStatView;

    [Header("Buttons")]
    [SerializeField] private Button btnExit;
    [SerializeField] private Button btnResume;


    private void Awake()
    {
        btnResume.onClick.AddListener(OnBtnResume);
        btnExit.onClick.AddListener(OnBtnExit);
    }
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }
    private void OnDisable()
    {
        Time.timeScale = 1f;
    }


    public void InitializePauseView(PlayerStat stat, List<Weapon> weaponList)
    {
        playerStatView.InitializePlayerStatView(stat);

        for (int i = 0; i < weaponList.Count; i++)
        {
            weaponStatView[i].InitializeWeaponStatUI(weaponList[i]);
        }

        gameObject.SetActive(true);
    }

    private void OnBtnResume()
        => gameObject.SetActive(false);

    private void OnBtnExit()
    {
        AddressableManager.Instance.ReleaseGroup("Stage1");
        LoadingSceneManager.LoadScene("MainMenuScene", "NULL", ESceneType.MainMenu);
    }
}
