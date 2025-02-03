using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnlockSystem : MonoBehaviour, ISaveData
{ 
    // 언락 리스트 (스테이지, 캐릭터)
    private List<bool> characterUnlockList = new List<bool>();
    private List<bool> stageUnlockList = new List<bool>();

    public IReadOnlyList<bool> CharacterUnlockList => characterUnlockList;
    public IReadOnlyList<bool> StageUnlockList => stageUnlockList;


    private void Awake()
    {
        // 캐릭터,스테이지 DB 가져와서 해금상태 초기화해주기

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
    // 저장된 리스트를 받아와 배열로 변환하고 그대로 대입해주어서 로드
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
        // 첫 로드시 언락데이터가 없으면 그대로 리턴
        // 리턴을 안하면 null 값으로 saveData가 저장되므로 게임 저장이 안됨
        if (saveData.UnlockData.stageUnlockList == null || saveData.UnlockData.characterUnlockList == null)
        {
            return;
        }

        characterUnlockList = saveData.UnlockData.characterUnlockList;
        stageUnlockList = saveData.UnlockData.stageUnlockList;
    }
}
