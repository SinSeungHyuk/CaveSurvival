using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelUpController : MonoBehaviour
{
    [SerializeField] private List<GradeSpriteData> btnSprites = new List<GradeSpriteData>();
    [SerializeField] private Sprite goldSprite;

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
    public void InitializeLevelUpUI(Color color, PlayerLevelUpData data, Sprite sprite, int btnIndex)
    {
        Sprite btnSprite = GetGradeSprite(data.ratio);

        btnLevelUps[btnIndex].InitializeBtnLevelUp(sprite, data.description, btnSprite);

        btnLevelUps[btnIndex].BtnLevelUp.onClick.AddListener(() => {
            GameManager.Instance.Player.Stat.PlayerStatChanged(data);
            gameObject.SetActive(false);
        });
    }

    // ���� ������ ������ (������ ������ 10�� �ƴ� ���)
    public void InitializeLevelUpUI(Color color, Weapon weapon, WeaponLevelUpData data, int btnIndex)
    {
        Sprite btnSprite = GetGradeSprite(data.ratio);

        btnLevelUps[btnIndex].InitializeBtnLevelUp(weapon.WeaponSprite, data.description, btnSprite);

        btnLevelUps[btnIndex].BtnLevelUp.onClick.AddListener(() => {
            weapon.WeaponStatChanged(data);
            gameObject.SetActive(false);
        });
    }

    // ���� ���׷��̵� ������ (������ ������ 10�� ������ ���)
    public void InitializeLevelUpUI(Weapon weapon, int btnIndex)
    {
        btnLevelUps[btnIndex].InitializeBtnLevelUp(weapon.WeaponSprite, weapon.WeaponDetails.upgradeDesc, goldSprite); // Ȳ�ݻ� �ؽ�Ʈ

        btnLevelUps[btnIndex].BtnLevelUp.onClick.AddListener(() => {
            weapon.UpgrageWeapon();
            gameObject.SetActive(false);
        });
    }

    // ���ο� ���� ������ (�÷��̾��� ���� ���� �����Ҷ�)
    public void InitializeLevelUpUI(WeaponDetailsSO weaponDetailsSO, int btnIndex)
    {
        Sprite btnSprite = GetGradeSprite(ELevelUpGrade.Normal);

        btnLevelUps[btnIndex].InitializeBtnLevelUp(weaponDetailsSO.weaponSprite, weaponDetailsSO.description, btnSprite);

        btnLevelUps[btnIndex].BtnLevelUp.onClick.AddListener(() => {
            GameManager.Instance.Player.AddWeaponToPlayer(weaponDetailsSO);
            gameObject.SetActive(false);
        });
    }


    private Sprite GetGradeSprite(ELevelUpGrade type)
    {
        Sprite sprite = null;

        sprite = btnSprites.FirstOrDefault(x => x.type == type).sprite;

        return sprite;
    }
}

[Serializable]
public struct GradeSpriteData
{
    public ELevelUpGrade type;
    public Sprite sprite;
}