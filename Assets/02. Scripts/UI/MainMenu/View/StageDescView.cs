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

    [SerializeField] private Button btnUnlock;

    private CurrencySystem currencySystem;


    public void InitializeStageDescView(StageDetailsSO stageDetailsSO, CurrencySystem currencySystem)
    {
        this.currencySystem = currencySystem;

        imgBoss.sprite = stageDetailsSO.bossSprite;

        txtStageName.text = stageDetailsSO.roomName;
        txtStageDesc.text = stageDetailsSO.roomDescription;
        
    }
}
