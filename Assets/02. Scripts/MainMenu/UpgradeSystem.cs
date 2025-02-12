using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour, ISaveData
{
    // ����,�ҷ����⸦ ���� ��ųʸ� <���� id, List<���Ⱥ� ����>>
    private SerializableDictionary<int, List<int>> upgradeDic = new SerializableDictionary<int, List<int>>();

    private Database weaponDB;

    public IReadOnlyDictionary<int, List<int>> UpgradeDic => upgradeDic;


    void Start()
    {
        weaponDB = AddressableManager.Instance.GetResource<Database>("DB_Weapon");
        List<int> startLevelList = new List<int> { 1, 1, 1, 1, 1, 1 };

        for (int i = 0; i < weaponDB.Count; i++)
        {
            upgradeDic.Add(i, startLevelList);
        }

        Register();
    }

    private void OnEnable()
    {
        SaveManager.Instance.OnLoadFinished += SaveManager_OnLoadFinished;
    }
    private void OnDisable()
    {
        SaveManager.Instance.OnLoadFinished -= SaveManager_OnLoadFinished;
    }


    private void SaveManager_OnLoadFinished()
    {
        Debug.Log("���� �̺�Ʈ ȣ��, �ε尡 �����ٴ� ��");

    }



    public void Register()
    {
        SaveManager.Instance.Register(this);
    }
    public void ToSaveData()
    {
        UpgradeSaveData unlockSaveData = new UpgradeSaveData()
        {
            upgradeDic = upgradeDic,
        };

        SaveManager.Instance.SaveData.UpgradeData = unlockSaveData;
    }
    public void FromSaveData(SaveData saveData)
    {
        this.upgradeDic = saveData.UpgradeData.upgradeDic;
    }
}
