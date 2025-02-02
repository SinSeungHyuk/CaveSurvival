using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageView : MonoBehaviour
{
    private List<BtnStageInfoUI> btnStageInfoUIs;
    private Database stageDB;


    private void Awake()
    {
        btnStageInfoUIs = GetComponentsInChildren<BtnStageInfoUI>().ToList();
        stageDB = AddressableManager.Instance.GetResource<Database>("DB_Stage");
    }

    public void InitializeStageView(CurrencySystem currencySystem, UnlockSystem unlockSystem)
    {
        for (int i = 0; i < btnStageInfoUIs.Count; i++)
        {
            // 언락 반영 구현


            btnStageInfoUIs[i].InitializeBtnStageInfoUI(stageDB.GetDataByID<StageDetailsSO>(i), currencySystem);
        }
    }
}