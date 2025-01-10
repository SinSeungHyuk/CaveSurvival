using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    public event Action OnMainGameStarted; // ī�޶� ������ ���� ���ӽ��� �̺�Ʈ

    [SerializeField] private PlayerDetailsSO playerSO; // �ӽ÷� ����ȭ. ���Ŀ� �����ؾ���
    [SerializeField] private StageDetailsSO stageSO; // �ӽ÷� ����ȭ. ���Ŀ� �����ؾ���


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

        // VCameraSetUp -> ī�޶� �¾����� �ʿ�
        OnMainGameStarted?.Invoke();
    }

}
