using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtHp;
    [SerializeField] private TextMeshProUGUI txtHpRegen;
    [SerializeField] private TextMeshProUGUI txtDefense;
    [SerializeField] private TextMeshProUGUI txtBonusDamage;
    [SerializeField] private TextMeshProUGUI txtMeleeDamage;
    [SerializeField] private TextMeshProUGUI txtRangeDamage;
    [SerializeField] private TextMeshProUGUI txtSpeed;
    [SerializeField] private TextMeshProUGUI txtDodge;
    [SerializeField] private TextMeshProUGUI txtPickUpRange;
    [SerializeField] private TextMeshProUGUI txtExpBonus;


    public void InitializePlayerStatView(PlayerStat stat)
    {
        txtHp.text = stat.MaxHp.ToString();
        txtHpRegen.text = stat.HpRegen.ToString();
        txtDefense.text = stat.Defense.ToString();
        txtBonusDamage.text = stat.BonusDamage.ToString();
        txtMeleeDamage.text = stat.MeleeDamage.ToString();
        txtRangeDamage.text = stat.RangeDamage.ToString();
        txtSpeed.text = stat.Speed.ToString("F1");
        txtDodge.text = stat.Dodge.ToString();
        txtPickUpRange.text = stat.PickUpRange.ToString();
        txtExpBonus.text = stat.ExpBonus.ToString();
    }
}
