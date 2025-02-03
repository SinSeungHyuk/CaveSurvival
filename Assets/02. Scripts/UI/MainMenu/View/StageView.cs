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
        // 스테이지 DB에서 ID를 하나씩 가져와 버튼에 넣어주기
        for (int i = 0; i < btnStageInfoUIs.Count; i++)
        {
            btnStageInfoUIs[i].InitializeBtnStageInfoUI(stageDB.GetDataByID<StageDetailsSO>(i), currencySystem, unlockSystem);
        }
    }
}