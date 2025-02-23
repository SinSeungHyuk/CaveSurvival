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



    #region SAVE & LOAD
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
        currencyList = saveData.CurrencyData.currencyList.ToArray();

        GetReward(); // 메인화면에 진입하고 로드할때마다 보상 받기
    }

    private void GetReward()
    {
        // 보상이 담긴 SO에 접근하여 재화에 추가
        RewardDataSO rewardData = AddressableManager.Instance.GetResource<RewardDataSO>("RewardData");
        IncreaseCurrency(ECurrencyType.Achive, rewardData.achiveReward);
        IncreaseCurrency(ECurrencyType.Gold, rewardData.goldReward);
    }
    #endregion
}