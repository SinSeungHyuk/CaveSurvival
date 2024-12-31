using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BtnWaveFinishUI : MonoBehaviour
{
    [SerializeField] private Image imgIcon;
    [SerializeField] private TextMeshProUGUI txtDescription;
    [SerializeField] private Button btnConfirm;

    public Button BtnConfirm => btnConfirm;


    public void InitializeBtnWaveFinish()
    {
        Player player = GameManager.Instance.Player;

        imgIcon.sprite = player.PlayerWaveBuff.WaveBuffList.LastOrDefault().BuffSprite;
        txtDescription.text = UtilitieHelper.GetStatType(player.PlayerWaveBuff.WaveBuffList.LastOrDefault().BuffType)
            + " + " + player.PlayerWaveBuff.WaveBuffList.LastOrDefault().Value;

        gameObject.SetActive(true);
    }
}
