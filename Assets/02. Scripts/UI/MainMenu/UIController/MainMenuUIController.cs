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
        // 테스트용 
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

        SetBtnOptions(); // 메인화면 버튼들 초기화
    }

    private void SetBtnOptions()
    {
        // 게임 종료 버튼
        btnExit.onClick.AddListener(() => Application.Quit());

        // 옵션버튼
        btnOption.onClick.AddListener(() => mainOptionController.InitializeMainOptionController());

        // 상점버튼
        btnShop.onClick.AddListener(() => shopController.InitializeShopController());
    }
}
