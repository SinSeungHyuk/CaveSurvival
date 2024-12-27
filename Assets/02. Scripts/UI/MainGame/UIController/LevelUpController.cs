using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelUpController : MonoBehaviour
{
    private List<BtnLevelUpUI> btnLevelUps;


    private void Awake()
    {
        btnLevelUps = GetComponentsInChildren<BtnLevelUpUI>().ToList();
    }
    private void OnEnable()
    {
        Time.timeScale = 0.0f;

        // ������ ��ư Ȱ��ȭ�Ǹ鼭 ������ �������� �����ֱ�
        foreach (var btnLevelUp in btnLevelUps)
            btnLevelUp.BtnLevelUp.onClick.RemoveAllListeners();
    }
    private void OnDisable()
    {
        Time.timeScale = 1.0f;
    }


    // �÷��̾� ������ ������
    public void InitializeLevelUpUI(PlayerLevelUpData data, Sprite sprite, int btnIndex)
    {
        btnLevelUps[btnIndex].InitializeBtnLevelUp(sprite, data.description);

        btnLevelUps[btnIndex].BtnLevelUp.onClick.AddListener(() => {
            GameManager.Instance.Player.Stat.PlayerStatChanged(data);
            gameObject.SetActive(false);
        });
    }

    // ���� ������ ������ (������ ������ 10�� �ƴ� ���)
    public void InitializeLevelUpUI(Weapon weapon, WeaponLevelUpData data, int btnIndex)
    {
        btnLevelUps[btnIndex].InitializeBtnLevelUp(weapon.WeaponSprite, data.description);

        btnLevelUps[btnIndex].BtnLevelUp.onClick.AddListener(() => {
            weapon.WeaponStatChanged(data);
            gameObject.SetActive(false);
        });
    }

    // ���� ���׷��̵� ������ (������ ������ 10�� ������ ���)
    public void InitializeLevelUpUI(Weapon weapon, int btnIndex)
    {
        btnLevelUps[btnIndex].InitializeBtnLevelUp(weapon.WeaponSprite, weapon.WeaponDetails.upgradeDesc);

        btnLevelUps[btnIndex].BtnLevelUp.onClick.AddListener(() => {
            weapon.UpgrageWeapon();
            gameObject.SetActive(false);
        });
    }

    // ���ο� ���� ������ (�÷��̾��� ���� ���� �����Ҷ�)
    public void InitializeLevelUpUI(WeaponDetailsSO weaponDetailsSO, int btnIndex)
    {
        btnLevelUps[btnIndex].InitializeBtnLevelUp(weaponDetailsSO.weaponSprite, weaponDetailsSO.description);
    }
}
