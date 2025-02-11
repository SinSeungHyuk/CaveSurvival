using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("Datas")]
    [SerializeField] private CurrencySystem currencySystem;
    [SerializeField] private UnlockSystem unlockSystem;

    [Header("Buttons")]
    [SerializeField] private Button btnStage;
    [SerializeField] private Button btnUpgrade;
    [SerializeField] private Button btnOption;
    [SerializeField] private Button btnExit;
    [SerializeField] private Button btnShop;

    [Header("BGM")]
    [SerializeField] private Button btnTest;
    [SerializeField] private MusicTrackSO mainmenuBGM;


    private CurrencyController currencyController;
    private MainStageController mainStageController;
    private ShopController shopController;
    private MainOptionController mainOptionController;


    private void Awake()
    {
        currencyController = GetComponent<CurrencyController>();
        mainStageController = GetComponent<MainStageController>();
        shopController = GetComponent<ShopController>();
        mainOptionController = GetComponent<MainOptionController>();

        SaveManager.Instance.OnLoadFinished += InitializeMainMenuUIController;
    }
    private void Start()
    {
        // �׽�Ʈ�� 
        btnTest.onClick.AddListener(() =>
        {
            currencySystem.IncreaseCurrency(ECurrencyType.Achive, 526);
            currencySystem.IncreaseCurrency(ECurrencyType.Gold, 1580);
            currencySystem.IncreaseCurrency(ECurrencyType.Diamond, 7);
        });

        MusicManager.Instance.PlayMusic(mainmenuBGM);

#if UNITY_EDITOR
        InitializeMainMenuUIController();
#endif
    }

    private void InitializeMainMenuUIController()
    {
        currencyController.InitializeCurrencyController(currencySystem);

        btnStage.onClick.AddListener(() 
            => mainStageController.InitializeMainStageController(currencySystem,unlockSystem));

        SetBtnOptions(); // ����ȭ�� ��ư�� �ʱ�ȭ
    }

    private void SetBtnOptions()
    {
        // ���� ���� ��ư
        btnExit.onClick.AddListener(() => Application.Quit());

        // �ɼǹ�ư
        btnOption.onClick.AddListener(() => mainOptionController.InitializeMainOptionController());

        // ������ư
        btnShop.onClick.AddListener(() => shopController.InitializeShopController());
    }
}
