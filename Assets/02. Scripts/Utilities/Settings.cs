
using UnityEngine;

public static class Settings
{
    #region GAME SETTING
    public static string regex = @"^(?=.*[A-Za-z])[A-Za-z0-9]{2,12}$"; // 닉네임 규칙
    public static int spawnInterval = 1; // 스폰간격 1초
    public static int waveTimer = 20; // 웨이브 지속시간 20초
    public static int extraTimePerWave = 5; // 웨이브당 지속시간 5초씩 증가
    public static int lastWave = 20; // 총 20개 웨이브 
    public static int stageBoundary = 15; // 스테이지 +- 크기 (정사각형이므로 -15 ~ 15)
    #endregion


    #region PLAYER SETTING
    public static int maxWeaponCount = 4; // 무기 최대 보유수 4
    public static int startExp = 25; // 1레벨 최대 경험치
    public static float expPerLevel = 0.1f; // 레벨당 증가 경험치
    public static int combatScalingConstant = 100; // 방어력 100일때 피해량 50% 감소
    public static int weaponUpgradeLevel = 9; // 무기는 9레벨에서 10레벨로 넘어갈때 업그레이드
    #endregion


    #region MONSTER SETTING
    public static int monsterFireRate = 2000; // 몬스터 공격간격 (밀리세컨드)
    public static float monsterProjectileDist = 20f; // 몬스터 투사체 사거리
    #endregion


    #region ANIMATOR PARAMETER

    #endregion


    #region LAYERMASK SETTING
    public static LayerMask monsterLayer = LayerMask.GetMask("Monster"); // 몬스터 레이어
    public static LayerMask itemDetectorLayer = LayerMask.GetMask("ItemDetector"); // 아이템 감지 레이어
    public static LayerMask itemPickUpLayer = LayerMask.GetMask("ItemPickUp"); // 아이템 획득 레이어
    #endregion


    #region COLOR SETTING
    public static Color32 green = new Color32(22, 135, 24, 255);
    public static Color32 beige = new Color32(207, 182, 151, 255);
    public static Color32 rare = new Color32(11, 110, 204, 255); // 파랑
    public static Color32 unique = new Color32(155, 61, 217, 255); // 보라
    public static Color32 legend = new Color32(255, 112, 120, 255); // 빨강
    public static Color32 critical = new Color32(255, 102, 2, 255); // 주황
    public static Color32 magnet = new Color32(255, 191, 0, 255); // 황금
    #endregion
}
