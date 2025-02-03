using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnlockSystem : MonoBehaviour, ISaveData
{ 
    // ��� ����Ʈ (��������, ĳ����)
    private List<bool> characterUnlockList = new List<bool>();
    private List<bool> stageUnlockList = new List<bool>();

    public IReadOnlyList<bool> CharacterUnlockList => characterUnlockList;
    public IReadOnlyList<bool> StageUnlockList => stageUnlockList;


    private void Awake()
    {
        // ĳ����,�������� DB �����ͼ� �رݻ��� �ʱ�ȭ���ֱ�

        var playerData = AddressableManager.Instance.GetResource<Database>("DB_Player");
        var stageData = AddressableManager.Instance.GetResource<Database>("DB_Stage");

        for (int i = 0; i < playerData.Count; i++)
        {
            var player = playerData.GetDataByID<PlayerDetailsSO>(i);
            characterUnlockList.Add(player.isUnlock);
        }
        for (int i = 0; i < stageData.Count; i++)
        {
            var stage = stageData.GetDataByID<StageDetailsSO>(i);
            stageUnlockList.Add(stage.isUnlock);
        }

        Register();
    }


    public void UnlockStage(int stageID, CurrencySystem currencySystem, int achive)
    {
        currencySystem.IncreaseCurrency(ECurrencyType.Achive, -achive);
        stageUnlockList[stageID] = true;
    }   

    public void UnlockCharacter(int characterID, CurrencySystem currencySystem, int achive)
    {
        currencySystem.IncreaseCurrency(ECurrencyType.Achive, -achive);
        characterUnlockList[characterID] = true;
    }

    public bool CanUnlockStage(int stageID, CurrencySystem currencySystem, int achive)
    {
        if (stageUnlockList[stageID] == false)
        {
            if (currencySystem.GetCurrency(ECurrencyType.Achive) >= achive)
            {
                return true;
            }
        }

        return false;
    }
    public bool CanUnlockChracter(int characterID, CurrencySystem currencySystem, int achive)
    {
        if (characterUnlockList[characterID] == false)
        {
            if (currencySystem.GetCurrency(ECurrencyType.Achive) >= achive)
            {
                return true;
            }
        }

        return false;
    }



    public void Register()
    {
        SaveManager.Instance.Register(this);
    }
    // ����� ����Ʈ�� �޾ƿ� �迭�� ��ȯ�ϰ� �״�� �������־ �ε�
    public void ToSaveData()
    {
        UnlockSaveData unlockSaveData = new UnlockSaveData() { 
            characterUnlockList = characterUnlockList,
            stageUnlockList = stageUnlockList
        };

        SaveManager.Instance.SaveData.UnlockData = unlockSaveData;
    }
    public void FromSaveData(SaveData saveData)
    {
        // ù �ε�� ��������Ͱ� ������ �״�� ����
        // ������ ���ϸ� null ������ saveData�� ����ǹǷ� ���� ������ �ȵ�
        if (saveData.UnlockData.stageUnlockList == null || saveData.UnlockData.characterUnlockList == null)
        {
            return;
        }

        characterUnlockList = saveData.UnlockData.characterUnlockList;
        stageUnlockList = saveData.UnlockData.stageUnlockList;
    }
}
