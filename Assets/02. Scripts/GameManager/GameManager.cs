using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    [SerializeField] private VCameraController vCam; 
    [SerializeField] private PostProcessingCtrl postProcessingCtrl; 

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

        // VCameraSetUp -> 카메라 셋업에서 필요
        vCam.InitializeVCam();
        // postProcessingCtrl 포스트 프로세싱 초기화
        postProcessingCtrl.InitializePostProcessingCtrl(Player);
    }
}
