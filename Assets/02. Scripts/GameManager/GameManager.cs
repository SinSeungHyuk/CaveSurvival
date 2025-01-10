using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    public event Action OnMainGameStarted; // 카메라 세팅을 위한 게임시작 이벤트

    [SerializeField] private PlayerDetailsSO playerSO; // 임시로 직렬화. 추후에 변경해야함
    [SerializeField] private StageDetailsSO stageSO; // 임시로 직렬화. 추후에 변경해야함


    public Player Player { get; private set; }
    public UIController UIController { get; private set; }


    /*
    private void OnEnable()
    {
        // TEST CODE @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
#if UNITY_EDITOR
        
        //Time.timeScale = 0.25f;
#endif
    }*/

    public void CreateMainGameScene()
    {
        StageManager.Instance.CreateStage(stageSO);

        Player = Instantiate(playerSO.player, Vector2.zero, Quaternion.identity).GetComponent<Player>();
        Player.InitializePlayer(playerSO);

        UIController = GameObject.FindWithTag("UIController").GetComponent<UIController>();
        UIController.InitializeUIController();

        // VCameraSetUp -> 카메라 셋업에서 필요
        OnMainGameStarted?.Invoke();
    }

}
