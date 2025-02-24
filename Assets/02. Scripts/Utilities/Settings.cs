
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    #region GAME SETTING
    public static string regex = @"^(?=.*[A-Za-z])[A-Za-z0-9]{2,12}$"; // �г��� ��Ģ
    public static int spawnInterval = 1; // �������� 1��
    public static int waveTimer = 20; // ���̺� ���ӽð� 20��
    public static int extraTimePerWave = 5; // ���̺�� ���ӽð� 5�ʾ� ����
    public static int lastWave = 19; // �� 20�� ���̺� 
    public static int stageBoundary = 15; // �������� +- ũ�� (���簢���̹Ƿ� -15 ~ 15)
    public static int rewardScaling = 2; // �������� ���� �� ���̺꺰 ������ ���� ����
    public const float musicFadeOutTime = 0.5f;
    public const float musicFadeInTime = 0.5f;
    #endregion


    #region PLAYER SETTING
    public static int maxWeaponCount = 4; // ���� �ִ� ������ 4
    public static int startExp = 40; // 1���� �ִ� ����ġ
    public static float expPerLevel = 0.1f; // ������ ���� ����ġ
    public static int combatScalingConstant = 100; // ���� 100�϶� ���ط� 50% ����
    public static int weaponUpgradeLevel = 9; // ����� 9�������� 10������ �Ѿ�� ���׷��̵�
    public static int hpRecoveryValue = 30; // ü��ȸ�� ������ ��ġ
    #endregion


    #region UPGRADE SETTING
    public static List<int> upgradgeDefaultLevel = new List<int> { 0, 0, 0, 0, 0, 0 }; // ���׷��̵� �⺻���� ����Ʈ (0����)

    public static int weaponDamageUpgrade = 1; // ���� ���ݷ� ���׷��̵� ��ġ (1�� ����)
    public static int weaponCriticChanceUpgrade = 1; // ���� ġ��Ȯ�� ���׷��̵� ��ġ (1�� ����)
    public static int weaponCriticDamageUpgrade = 5; // ���� ġ������ ���׷��̵� ��ġ (5�� ����)
    public static int weaponFireRateUpgrade = 3; // ���� ���ݼӵ� ���׷��̵� ��ġ (3%�� ����)
    public static int weaponRangeUpgrade = 3; // ���� ��Ÿ� ���׷��̵� ��ġ (3%�� ����)
    public static int weaponKnockbackUpgrade = 10; // ���� �˹� ���׷��̵� ��ġ (10�� ����)

    public static int weaponDamageUpgradeGold = 1000; // ���ݷ� ���׷��̵� ���
    public static int weaponCriticChanceUpgradeGold = 1500;
    public static int weaponCriticDamageUpgradeGold = 1500;
    public static int weaponFireRateUpgradeGold = 1000;
    public static int weaponRangeUpgradeGold = 700; 
    public static int weaponKnockbackUpgradeGold = 500;
    #endregion


    #region MONSTER SETTING
    public static int monsterFireRate = 2100; // ���� ���ݰ��� (�и�������)
    public static float monsterProjectileDist = 30f; // ���� ����ü ��Ÿ�
    #endregion


    #region ANIMATOR PARAMETER

    #endregion


    #region LAYERMASK SETTING
    public static LayerMask monsterLayer = LayerMask.GetMask("Monster"); // ���� ���̾�
    public static LayerMask playerLayer = LayerMask.GetMask("PlayerCollider"); // �÷��̾� ���̾�
    public static LayerMask itemDetectorLayer = LayerMask.GetMask("ItemDetector"); // ������ ���� ���̾�
    public static LayerMask itemPickUpLayer = LayerMask.GetMask("ItemPickUp"); // ������ ȹ�� ���̾�
    #endregion


    #region COLOR SETTING
    public static Color32 green = new Color32(22, 135, 24, 255);
    public static Color32 beige = new Color32(207, 182, 151, 255);
    public static Color32 rare = new Color32(11, 110, 204, 255); // �Ķ�
    public static Color32 unique = new Color32(155, 61, 217, 255); // ����
    public static Color32 legend = new Color32(255, 112, 120, 255); // ����
    public static Color32 critical = new Color32(255, 102, 2, 255); // ��Ȳ
    public static Color32 magnet = new Color32(255, 191, 0, 255); // Ȳ��
    #endregion


    #region UI TEXT SETTING
    public static string unlockText = "��� ����";
    #endregion
}
