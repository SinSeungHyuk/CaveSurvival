using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    [Header("Main Game Scene")]
    [SerializeField] private VCameraController vCam; 
    [SerializeField] private PostProcessingCtrl postProcessingCtrl;

    [Header("Main Menu Scene")]
    [SerializeField] private CurrencySystem currencySystem;


    private StageCharacterDataSO stageCharacterData;


    public Player Player { get; private set; }
    public UIController UIController { get; private set; }
    public VCameraController VCam => vCam;



    public void CreateMainGameScene()
    {
        stageCharacterData = AddressableManager.Instance.GetResource<StageCharacterDataSO>("StageCharacterData");

        StageManager.Instance.CreateStage(stageCharacterData.stageDetails);

        Player = Instantiate(stageCharacterData.playerDetails.player, Vector2.zero, Quaternion.identity).GetComponent<Player>();
        Player.InitializePlayer(stageCharacterData.playerDetails);

        UIController = GameObject.FindWithTag("UIController").GetComponent<UIController>();
        UIController.InitializeUIController();

        // VCameraSetUp -> ī�޶� �¾����� �ʿ�
        vCam.InitializeVCam();
        // postProcessingCtrl ����Ʈ ���μ��� �ʱ�ȭ
        postProcessingCtrl.InitializePostProcessingCtrl(Player);
    }

    //public void CreateMainMenuScene()
    //{
    //    RewardDataSO rewardData = AddressableManager.Instance.GetResource<RewardDataSO>("RewardData");

    //    currencySystem.IncreaseCurrency(ECurrencyType.Achive, rewardData.achiveReward);
    //    currencySystem.IncreaseCurrency(ECurrencyType.Gold, rewardData.goldReward);
    //}
}
