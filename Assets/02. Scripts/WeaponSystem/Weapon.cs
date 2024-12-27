using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class Weapon : MonoBehaviour
{
    private WeaponTypeDetailsSO upgradeType;

    public WeaponDetailsSO WeaponDetails { get; private set; }
    public string WeaponName { get; private set; }
    public WeaponTypeDetailsSO WeaponType { get; private set; }
    public WeaponDetectorSO DetectorType { get; private set; }
    public string WeaponParticle { get; private set; }
    public Sprite WeaponSprite { get; private set; }
    public Player Player { get; set; } // ���⸦ ������ �÷��̾�


    // ������ ���� (�������Ͽ� ���� ��� ����)
    public int WeaponLevel { get; private set; } // ���� ����
    public int WeaponDamage { get; private set; } // ������
    public int WeaponCriticChance { get; private set; } // ġ��Ÿ Ȯ�� (%)
    public int WeaponCriticDamage { get; private set; } // ġ��Ÿ ���� (%)
    public float WeaponFireRate { get; private set; } // ���ݼӵ�
    public float WeaponRange { get; private set; } // ��Ÿ�
    public int WeaponKnockback { get; private set; } // �˹�Ÿ�


    // �ǽð����� �ΰ��ӿ��� ����ϸ鼭 �ٲ�� ����
    public float WeaponFireRateTimer; // ���� ��Ÿ��


    private void Update()
    {
        // ����ӵ� 
        if (WeaponFireRateTimer > 0f)
            WeaponFireRateTimer -= Time.deltaTime;
        else WeaponFireRateTimer = WeaponFireRate;


        if (Input.GetKeyUp(KeyCode.E))
        {
            UpgrageWeapon();
        }
    }

    public void InitializeWeapon(WeaponDetailsSO weaponDetails) // ���� �ʱ�ȭ
    {
        WeaponDetails = weaponDetails; // �Ҹ�,����Ʈ � �����ϱ� ����
        WeaponName = weaponDetails.weaponName;
        WeaponType = weaponDetails.weaponType;
        upgradeType = weaponDetails.upgradeType;
        DetectorType = weaponDetails.detectorType;
        WeaponParticle = weaponDetails.weaponParticle.ToString();
        WeaponSprite = weaponDetails.weaponSprite;

        WeaponLevel = 1;
        WeaponDamage = weaponDetails.weaponBaseDamage;
        WeaponCriticChance = weaponDetails.weaponCriticChance;
        WeaponCriticDamage = weaponDetails.weaponCriticDamage;
        WeaponFireRate = weaponDetails.weaponFireRate;
        WeaponRange = weaponDetails.weaponRange;
        WeaponKnockback = weaponDetails.weaponKnockback;

        WeaponFireRateTimer = weaponDetails.weaponFireRate; // ���ݼӵ� �ʱ�ȭ
    }


    /// ���� ������ �Լ� ����
    #region WEAPON LEVEL UP
    public void UpgrageWeapon()
    {
        // ������ ���� ���
        WeaponLevel++;

        WeaponType = upgradeType;
    }

    public void WeaponStatChanged(WeaponLevelUpData data)
    {
        // ������ ���� ���
        WeaponLevel++;

        switch (data.statType)
        {
            case EWeaponStatType.WeaponDamage:
                // ���� �⺻ ������ ó��
                WeaponDamage = UtilitieHelper.IncreaseByPercent(WeaponDamage, data.value);
                break;
            case EWeaponStatType.WeaponCriticChance:
                // ġ��Ÿ Ȯ�� ó��
                WeaponCriticChance += data.value;
                break;
            case EWeaponStatType.WeaponCriticDamage:
                // ġ��Ÿ ������ ó��
                WeaponCriticDamage += data.value;
                break;
            case EWeaponStatType.WeaponFireRate:
                // ���� �ӵ� ó��
                WeaponFireRate = UtilitieHelper.DecreaseByPercent(WeaponFireRate, data.value);
                break;
            case EWeaponStatType.WeaponRange:
                // ���� �����Ÿ� ó��
                WeaponRange = UtilitieHelper.IncreaseByPercent(WeaponRange, data.value);
                break;
            case EWeaponStatType.WeaponKnockback:
                // �˹� ȿ�� ó��
                WeaponKnockback += data.value;
                break;
            default:
                // �⺻ ó��
                break;
        }
    } 
    #endregion
}
