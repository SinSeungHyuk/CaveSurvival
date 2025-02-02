using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStageController : MonoBehaviour
{
    [SerializeField] private MainStageView mainStageView;


    public void InitializeMainStageController(CurrencySystem currencySystem, UnlockSystem unlockSystem)
    {
        mainStageView.gameObject.SetActive(true);

        mainStageView.InitializeMainStageView(currencySystem, unlockSystem);
    }
}
