using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    [Header("Main Game Scene")]
    [SerializeField] private VCameraController vCam; 
    [SerializeField] private PostProcessingCtrl postProcessingCtrl;


    public Player Player { get; private set; }
    public UIController UIController { get; private set; }
    public VCameraController VCam => vCam;



    public void CreateMainGameScene()
    {
        var stageCharacterData = AddressableManager.Instance.GetResource<StageCharacterDataSO>("StageCharacterData");

        StageManager.Instance.CreateStage(stageCharacterData.stageDetails);

        Player = Instantiate(stageCharacterData.playerDetails.player, Vector2.zero, Quaternion.identity).GetComponent<Player>();
        Player.InitializePlayer(stageCharacterData.playerDetails);

        UIController = GameObject.FindWithTag("UIController").GetComponent<UIController>();
        UIController.InitializeUIController();

        // InitializeVCam -> ī�޶� �¾�
        vCam.InitializeVCam();
        // postProcessingCtrl ����Ʈ ���μ��� �ʱ�ȭ
        postProcessingCtrl.InitializePostProcessingCtrl(Player);
    }
}
