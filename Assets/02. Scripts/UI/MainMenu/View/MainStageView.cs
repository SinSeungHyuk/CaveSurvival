using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainStageView : MonoBehaviour
{
    [SerializeField] private StageView stageView;
    [SerializeField] private CharacterView characterView;

    [SerializeField] private CharacterDescView characterDescView;
    [SerializeField] private StageDescView stageDescView;
    [SerializeField] private Button btnExit;



    public void InitializeMainStageView(CurrencySystem currencySystem, UnlockSystem unlockSystem)
    {
        stageView.InitializeStageView(currencySystem, unlockSystem);
        characterView.InitializeCharacterView(currencySystem, unlockSystem);

        btnExit.onClick.RemoveAllListeners();
        btnExit.onClick.AddListener(ExitMainStage);
    }

    private void ExitMainStage()
    {
        characterDescView.gameObject.SetActive(false);
        stageDescView.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }
}
