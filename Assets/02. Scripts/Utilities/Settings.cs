
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    #region GAME SETTING
    public static string regex = @"^(?=.*[A-Za-z])[A-Za-z0-9]{2,12}$"; // 닉네임 규칙
    public static int spawnInterval = 1; // 스폰간격 1초
    public static int waveTimer = 20; // 웨이브 지속시간 20초
    public static int extraTimePerWave = 5; // 웨이브당 지속시간 5초씩 증가
    public static int lastWave = 19; // 총 20개 웨이브 
    public static int stageBoundary = 15; // 스테이지 +- 크기 (정사각형이므로 -15 ~ 15)
    public static int rewardScaling = 2; // 스테이지 종료 후 웨이브별 보상의 성장곡선 지수
    public const float musicFadeOutTime = 0.5f;
    public const float musicFadeInTime = 0.5f;
    #endregion


    #region PLAYER SETTING
    public static int maxWeaponCount = 4; // 무기 최대 보유수 4
    public static int startExp = 40; // 1레벨 최대 경험치
    public static float expPerLevel = 0.1f; // 레벨당 증가 경험치
    public static int combatScalingConstant = 100; // 방어력 100일때 피해량 50% 감소
    public static int weaponUpgradeLevel = 9; // 무기는 9레벨에서 10레벨로 넘어갈때 업그레이드
    public static int hpRecoveryValue = 30; // 체력회복 아이템 수치
    #endregion


    #region UPGRADE SETTING
    public static List<int> upgradgeDefaultLevel = new List<int> { 0, 0, 0, 0, 0, 0 }; // 업그레이드 기본레벨 리스트 (0레벨)

    public static int weaponDamageUpgrade = 1; // 무기 공격력 업그레이드 수치 (1씩 증가)
    public static int weaponCriticChanceUpgrade = 1; // 무기 치명확률 업그레이드 수치 (1씩 증가)
    public static int weaponCriticDamageUpgrade = 5; // 무기 치명데미지 업그레이드 수치 (5씩 증가)
    public static int weaponFireRateUpgrade = 3; // 무기 공격속도 업그레이드 수치 (3%씩 증가)
    public static int weaponRangeUpgrade = 3; // 무기 사거리 업그레이드 수치 (3%씩 증가)
    public static int weaponKnockbackUpgrade = 10; // 무기 넉백 업그레이드 수치 (10씩 증가)

    public static int weaponDamageUpgradeGold = 1000; // 공격력 업그레이드 비용
    public static int weaponCriticChanceUpgradeGold = 1500;
    public static int weaponCriticDamageUpgradeGold = 1500;
    public static int weaponFireRateUpgradeGold = 1000;
    public static int weaponRangeUpgradeGold = 700; 
    public static int weaponKnockbackUpgradeGold = 500;
    #endregion


    #region MONSTER SETTING
    public static int monsterFireRate = 2100; // 몬스터 공격간격 (밀리세컨드)
    public static float monsterProjectileDist = 30f; // 몬스터 투사체 사거리
    #endregion


    #region ANIMATOR PARAMETER

    #endregion


    #region LAYERMASK SETTING
    public static LayerMask monsterLayer = LayerMask.GetMask("Monster"); // 몬스터 레이어
    public static LayerMask playerLayer = LayerMask.GetMask("PlayerCollider"); // 플레이어 레이어
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


    #region UI TEXT SETTING
    public static string unlockText = "잠금 해제";
    #endregion
}
