using R3;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtExp;

    private PlayerStat playerStat;
    private ExpViewModel expViewModel;
    private Slider expBar;


    private void Awake()
    {
        expBar = GetComponent<Slider>();
    }

    public void InitializeExpView()
    {
        playerStat = GameManager.Instance.Player.Stat;
        expViewModel = new ExpViewModel(playerStat);

        expViewModel.ExpPercentage.Subscribe(expPercentage
            => SetExpBar(expPercentage));
    }

    private void SetExpBar(float ratio) 
    {
        expBar.value = ratio;
        txtExp.text = $"{(ratio * 100):F1} %";
    }
}
