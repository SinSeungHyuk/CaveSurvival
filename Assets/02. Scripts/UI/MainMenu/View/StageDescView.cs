using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageDescView : MonoBehaviour
{
    [SerializeField] private Image imgBoss;
    [SerializeField] private TextMeshProUGUI txtStageName;
    [SerializeField] private TextMeshProUGUI txtStageDesc;
    [SerializeField] private TextMeshProUGUI txtUnlockCost;
    [SerializeField] private Button btnUnlock;

    private CurrencySystem currencySystem;
    private StageDetailsSO stageDetailsSO;
    private UnlockSystem unlockSystem;


    public void InitializeStageDescView(StageDetailsSO stageDetailsSO, CurrencySystem currencySystem, UnlockSystem unlockSystem)
    {
        this.stageDetailsSO = stageDetailsSO;
        this.currencySystem = currencySystem;
        this.unlockSystem = unlockSystem;

        SetStageDescUI();
        SetBtnUnlock();
    }

    private void SetStageDescUI()
    {
        imgBoss.sprite = stageDetailsSO.bossSprite;

        txtStageName.text = stageDetailsSO.roomName;
        txtStageDesc.text = stageDetailsSO.roomDescription;
        txtUnlockCost.text = stageDetailsSO.unlockCost.ToString();
    }

    private void SetBtnUnlock()
    {
        // 언락 버튼은 해당 캐릭터가 언락이 가능한 상황일때만 누를 수 있음 (Achive 보유량)
        // 언락 버튼을 클릭하면 언락코스트만큼 재화가 소모되고 캐릭터가 해제되면서 선택됨
        btnUnlock.enabled = false;
        if (unlockSystem.StageUnlockList[stageDetailsSO.ID] == false)
        {
            if (unlockSystem.CanUnlockStage(stageDetailsSO.ID, currencySystem, stageDetailsSO.unlockCost))
                btnUnlock.enabled = true;

            btnUnlock.onClick.RemoveAllListeners();
            btnUnlock.onClick.AddListener(() =>
            {
                UnlockStage();
            });
        }
        else
        {
            txtUnlockCost.text = Settings.unlockText;
        }
    }

    private void UnlockStage()
    {
        unlockSystem.UnlockStage(stageDetailsSO.ID, currencySystem, stageDetailsSO.unlockCost);
        txtUnlockCost.text = Settings.unlockText;
        var stageCharacterData = AddressableManager.Instance.GetResource<StageCharacterDataSO>("StageCharacterData");
        stageCharacterData.stageDetails = stageDetailsSO;
        btnUnlock.enabled = false;
    }
}
