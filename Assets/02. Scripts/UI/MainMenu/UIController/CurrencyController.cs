using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    [SerializeField] private CurrencyView currencyView;

    private CurrencySystem currencySystem;


    private void OnDestroy()
    {
        if (currencySystem != null)
        {
            currencySystem.OnCurrencyChanged -= CurrencySystem_OnCurrencyChanged;
        }
    }

    public void InitializeCurrencyController(CurrencySystem currencySystem)
    {
        this.currencySystem = currencySystem;

        this.currencySystem.OnCurrencyChanged += CurrencySystem_OnCurrencyChanged;

        // 처음 메인화면에 진입할때 최초로 한번 화폐 로드해주기
        foreach (ECurrencyType type in Enum.GetValues(typeof(ECurrencyType)))
        {
            int value = currencySystem.GetCurrency(type);
            currencyView.SetTxtCurrency(type, value);
        }
    }

    private void CurrencySystem_OnCurrencyChanged(CurrencySystem @event, ECurrencyType type, int value)
    {
        currencyView.SetTxtCurrency(type, value);
    }
}
