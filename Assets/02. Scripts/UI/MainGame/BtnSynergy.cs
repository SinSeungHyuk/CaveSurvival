using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BtnSynergy : MonoBehaviour
{
    [SerializeField] private Image gradeImage;
    [SerializeField] private Image synergyImage;
    [SerializeField] private TextMeshProUGUI txtSynergy;
    [SerializeField] private GameObject popupUI;

    private Button btnSynergy;


    public void InitializeBtnSynergy(SynergyDetailsSO synergyData)
    {
        btnSynergy = GetComponent<Button>();

        gradeImage.sprite = synergyData.synergyGrade;
        synergyImage.sprite = synergyData.synergyIcon;
        txtSynergy.text = synergyData.synergyDesc;

        popupUI.SetActive(false);

        btnSynergy.onClick.AddListener(SetBtnSynergy);
    }

    private void SetBtnSynergy()
    {
        bool isOpenPopup = (popupUI.activeSelf) ? false : true;
        popupUI.SetActive(isOpenPopup);
    }
}
