using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CurrencySystem : MonoBehaviour, ICurrencyService, ISaveData
{
    public event Action<CurrencySystem, ECurrencyType, int> OnCurrencyChanged;


    // �迭�� ������ְ� �ʱ�ȭ�ϸ� �ε��� �ٷ� ����
    private int[] currencyList = new int[3];

    public int[] CurrencyList => currencyList;


    private void Start()
    {
        Register();

        Debug.Log("��ȭ ����");

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
    // ����� ����Ʈ�� �޾ƿ� �迭�� ��ȯ�ϰ� �״�� �������־ �ε�
    public void ToSaveData()
    {
        CurrencySaveData currencySaveData = new CurrencySaveData() { currencyList = currencyList.ToList() };

        SaveManager.Instance.SaveData.CurrencyData = currencySaveData;
    }
    public void FromSaveData(SaveData saveData)
    {
        Debug.Log("��ȭ �ε�");
        currencyList = saveData.CurrencyData.currencyList.ToArray();

    }
}