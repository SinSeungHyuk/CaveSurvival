using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


[Serializable]
public struct SaveData
{
    public CurrencySaveData CurrencyData;
    public UnlockSaveData UnlockData;
}


[Serializable]
public struct CurrencySaveData // 화폐 저장
{
    public List<int> currencyList;
}

[Serializable]
public struct UnlockSaveData // 해금리스트 저장
{
    public List<bool> characterUnlockList;
    public List<bool> stageUnlockList;
}