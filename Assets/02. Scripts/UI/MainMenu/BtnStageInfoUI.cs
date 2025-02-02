using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnStageInfoUI : MonoBehaviour
{
    private Button btnStageInfoUI;

    [SerializeField] private CharacterDescView characterDescView;
    [SerializeField] private StageDescView stageDescView;


    private void Awake()
    {
        btnStageInfoUI = GetComponent<Button>();
    }

    public void InitializeBtnStageInfoUI(StageDetailsSO stageDetailsSO, CurrencySystem currencySystem)
    {
        var stageCharacterData = AddressableManager.Instance.GetResource<StageCharacterDataSO>("StageCharacterData");

        stageCharacterData.stageDetails = stageDetailsSO;

        btnStageInfoUI.onClick.RemoveAllListeners();
        btnStageInfoUI.onClick.AddListener(()
            => InitializeStageDescView(stageDetailsSO, currencySystem));
    }

    private void InitializeStageDescView(StageDetailsSO stageDetailsSO, CurrencySystem currencySystem)
    {
        var stageCharacterData = AddressableManager.Instance.GetResource<StageCharacterDataSO>("StageCharacterData");
        stageCharacterData.stageDetails = stageDetailsSO;

        stageDescView.InitializeStageDescView(stageDetailsSO, currencySystem);
        stageDescView.gameObject.SetActive(true);
        characterDescView.gameObject.SetActive(false);
    }
}