using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStatView : MonoBehaviour
{
    [SerializeField] private Image weaponSprite;
    [SerializeField] private TextMeshProUGUI txtWeaponDamage;
    [SerializeField] private TextMeshProUGUI txtWeaponCriticChance;
    [SerializeField] private TextMeshProUGUI txtWeaponCriticDamage;
    [SerializeField] private TextMeshProUGUI txtWeaponFireRate;
    [SerializeField] private TextMeshProUGUI txtWeaponRange;
    [SerializeField] private TextMeshProUGUI txtWeaponKnockback;


    public void InitializeWeaponStatView(Weapon weapon)
    {
        weaponSprite.sprite = weapon.WeaponSprite;
        txtWeaponDamage.text = weapon.WeaponDamage.ToString();
        txtWeaponCriticChance.text = weapon.WeaponCriticChance.ToString() + " %";
        txtWeaponCriticDamage.text = weapon.WeaponCriticDamage.ToString() + " %";
        txtWeaponFireRate.text = weapon.WeaponFireRate.ToString("F2") + " /s";
        txtWeaponRange.text = weapon.WeaponRange.ToString();
        txtWeaponKnockback.text = weapon.WeaponKnockback.ToString();
    }
}