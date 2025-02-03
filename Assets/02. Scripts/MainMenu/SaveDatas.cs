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
public struct CurrencySaveData // ȭ�� ����
{
    public List<int> currencyList;
}

[Serializable]
public struct UnlockSaveData // �رݸ���Ʈ ����
{
    public List<bool> characterUnlockList;
    public List<bool> stageUnlockList;
}