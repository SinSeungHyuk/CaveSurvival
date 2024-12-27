
using UnityEngine;

public static class Settings
{
    #region GAME SETTING
    public static string regex = @"^(?=.*[A-Za-z])[A-Za-z0-9]{2,12}$"; // �г��� ��Ģ
    public static int spawnInterval = 10; // �������� 1��
    public static int waveTimer = 20; // ���̺� ���ӽð� 20��
    public static int extraTimePerWave = 5; // ���̺�� ���ӽð� 5�ʾ� ����
    public static int stageBoundary = 15; // �������� +- ũ�� (���簢���̹Ƿ� -15 ~ 15)
    #endregion


    #region PLAYER SETTING
    public static int maxWeaponCount = 4; // ���� �ִ� ������ 4
    public static int startExp = 25; // 1���� �ִ� ����ġ
    public static float expPerLevel = 0.1f; // ������ ���� ����ġ
    public static int combatScalingConstant = 100; // ���� 100�϶� ���ط� 50% ����
    public static int weaponUpgradeLevel = 9; // ����� 9�������� 10������ �Ѿ�� ���׷��̵�
    #endregion


    #region MONSTER SETTING
    public static int monsterFireRate = 2000; // ���� ���ݰ��� (�и�������)
    public static float monsterProjectileDist = 20f; // ���� ����ü ��Ÿ�
    #endregion


    #region ANIMATOR PARAMETER

    #endregion


    #region LAYERMASK SETTING
    public static LayerMask monsterLayer = LayerMask.GetMask("Monster"); // ���� ���̾�
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
    #endregion
}
