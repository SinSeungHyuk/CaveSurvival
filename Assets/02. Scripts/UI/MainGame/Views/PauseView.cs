using GooglePlayGames.BasicApi;
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
    [SerializeField] private BtnSynergy btnSynergy;
    [SerializeField] private Transform synergyTransform;


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
        foreach (Transform btnSynergy in synergyTransform)
        {
            Destroy(btnSynergy.gameObject);
        }

        Time.timeScale = 1f;
    }


    public void InitializePauseView(Player player)
    {
        var stat = player.Stat;
        var weaponList = player.WeaponList;

        playerStatView.InitializePlayerStatView(stat);

        for (int i = 0; i < weaponList.Count; i++)
        {
            weaponStatView[i].InitializeWeaponStatUI(weaponList[i]);
        }

        CreateBtnSynergy(player);

        gameObject.SetActive(true);
    }

    private void CreateBtnSynergy(Player player)
    {
        foreach (var synergyData in player.PlayerSynergy.SynergyList)
        {
            var BtnSynergy = Instantiate(btnSynergy, synergyTransform).GetComponent<BtnSynergy>();
            BtnSynergy.InitializeBtnSynergy(synergyData);
        }
    }

    private void OnBtnResume()
        => gameObject.SetActive(false);

    private void OnBtnExit()
    {
        RewardDataSO rewardData = AddressableManager.Instance.GetResource<RewardDataSO>("RewardData");
        rewardData.achiveReward = 0;
        rewardData.goldReward = 0;

        AdManager.Instance.ShowInterstitialAd();
    }
}
