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


    public void InitializeStageDescView(StageDetailsSO stageDetailsSO, CurrencySystem currencySystem, UnlockSystem unlockSystem)
    {
        this.currencySystem = currencySystem;

        imgBoss.sprite = stageDetailsSO.bossSprite;

        txtStageName.text = stageDetailsSO.roomName;
        txtStageDesc.text = stageDetailsSO.roomDescription;
        txtUnlockCost.text = stageDetailsSO.unlockCost.ToString();

        // ��� ��ư�� �ش� ĳ���Ͱ� ����� ������ ��Ȳ�϶��� ���� �� ���� (Achive ������)
        // ��� ��ư�� Ŭ���ϸ� ����ڽ�Ʈ��ŭ ��ȭ�� �Ҹ�ǰ� ĳ���Ͱ� �����Ǹ鼭 ���õ�
        btnUnlock.enabled = false;
        if (unlockSystem.StageUnlockList[stageDetailsSO.ID] == false)
        {
            if (unlockSystem.CanUnlockStage(stageDetailsSO.ID, currencySystem, stageDetailsSO.unlockCost))
                btnUnlock.enabled = true;

            btnUnlock.onClick.RemoveAllListeners();
            btnUnlock.onClick.AddListener(() =>
            {
                unlockSystem.UnlockStage(stageDetailsSO.ID, currencySystem, stageDetailsSO.unlockCost);
                var stageCharacterData = AddressableManager.Instance.GetResource<StageCharacterDataSO>("StageCharacterData");
                stageCharacterData.stageDetails = stageDetailsSO;
                btnUnlock.enabled = false;
            });
        }
        else
        {
            txtUnlockCost.text = "������";
        }
    }
}
