using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class CurrencyView : MonoBehaviour
{
    [SerializeField] private List<CurrencyViewUI> currencyViewUIs = new List<CurrencyViewUI>();

    [Serializable]
    private struct CurrencyViewUI
    {
        public ECurrencyType type;
        public TextMeshProUGUI txtCurrency;
    }


    public void SetTxtCurrency(ECurrencyType type, int value)
    {
        currencyViewUIs.FirstOrDefault(x => x.type == type).txtCurrency.text = value.ToString("N0");
    }
}
