using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("Datas")]
    [SerializeField] private CurrencySystem currencySystem;

    [Header("Test")]
    [SerializeField] private Button btnTest;


    private CurrencyController currencyController;


    private void Awake()
    {
        currencyController = GetComponent<CurrencyController>();

        SaveManager.Instance.OnLoadFinished += InitializeMainMenuUIController;
    }
    private void Start()
    {
        // 테스트용 
        btnTest.onClick.AddListener(() =>
        {
            currencySystem.IncreaseCurrency(ECurrencyType.Achive, 34);
            currencySystem.IncreaseCurrency(ECurrencyType.Gold, 1580);
            currencySystem.IncreaseCurrency(ECurrencyType.Diamond, 7);
        });
    }

    private void InitializeMainMenuUIController()
    {
        currencyController.InitializeCurrencyController(currencySystem);
    }
}
