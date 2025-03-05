using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BtnLevelUpUI : MonoBehaviour
{
    [SerializeField] private Image imgSprite;
    [SerializeField] private Image btnSprite;
    [SerializeField] private Image imgWeaponElement;
    [SerializeField] private Image imgWeaponType;
    [SerializeField] private TextMeshProUGUI txtDescription;
    private Button btnLevelUp;

    public Button BtnLevelUp => btnLevelUp;


    private void Awake()
    {
        btnLevelUp = GetComponent<Button>();
    }

    public void InitializeBtnLevelUp(Sprite sprite, string desc, Sprite btnSprite)
    {
        imgSprite.sprite = sprite;
        this.btnSprite.sprite = btnSprite;
        txtDescription.text = desc;
        imgWeaponElement.enabled = false;
        imgWeaponType.enabled = false;
    }

    public void InitializeBtnLevelUp(WeaponDetailsSO weaponDetails, string desc, Sprite btnSprite)
    {
        imgSprite.sprite = weaponDetails.weaponSprite;
        this.btnSprite.sprite = btnSprite;
        txtDescription.text = desc;
        imgWeaponElement.enabled = true;
        imgWeaponType.enabled = true;
        imgWeaponElement.sprite = weaponDetails.weaponElementIcon;
        imgWeaponType.sprite = weaponDetails.weaponAttackTypeIcon;
    }
}
