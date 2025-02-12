using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CurrencySystem : MonoBehaviour, ICurrencyService, ISaveData
{
    public event Action<CurrencySystem, ECurrencyType, int> OnCurrencyChanged;


    // 배열은 사이즈넣고 초기화하면 인덱싱 바로 가능
    private int[] currencyList = new int[3];

    public int[] CurrencyList => currencyList;


    private void Start()
    {
        Register();

        Debug.Log("재화 시작");

    }


    public void IncreaseCurrency(ECurrencyType type, int amount)
    {
        currencyList[Convert.ToInt32(type)] += amount;
        OnCurrencyChanged?.Invoke(this, type, currencyList[Convert.ToInt32(type)]);
    }

    public int GetCurrency(int type)
        => currencyList[type];
    public int GetCurrency(ECurrencyType type)
        => currencyList[Convert.ToInt32(type)];



    public void Register()
    {
        SaveManager.Instance.Register(this);
    }
    // 저장된 리스트를 받아와 배열로 변환하고 그대로 대입해주어서 로드
    public void ToSaveData()
    {
        CurrencySaveData currencySaveData = new CurrencySaveData() { currencyList = currencyList.ToList() };

        SaveManager.Instance.SaveData.CurrencyData = currencySaveData;
    }
    public void FromSaveData(SaveData saveData)
    {
        Debug.Log("재화 로드");
        currencyList = saveData.CurrencyData.currencyList.ToArray();

    }
}