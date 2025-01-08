using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


// 전역으로 접근가능한 각종 계산 static 클래스
public static class UtilitieHelper
{
    // 벡터로부터 각도 구하기  ===========================================================
    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x); // Atan2(y,x) 라디안 구하기
        float degrees = radians * Mathf.Rad2Deg;         // 라디안 디그리로 변환

        return degrees;
    }

    // 각도로부터 방향벡터 구하기  ===========================================================
    public static Vector3 GetDirectionVectorFromAngle(float angle)
    {
        Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
        return direction;
    }

    // 확률 계산하기  ===========================================================
    public static bool isSuccess(int percent)
    {
        int chance = Random.Range(1, 101);
        return percent >= chance; // 성공하면 true
    }
    public static bool isSuccess(float percent)
    {
        int chance = Random.Range(1, 101);
        return percent >= chance; // 성공하면 true
    }

    // 퍼센트 % 계산하기  ===========================================================
    public static int IncreaseByPercent(int value, float percent)
    {
        float increase = value * (percent / 100);
        return value + (int)increase;
    }
    public static float IncreaseByPercent(float value, float percent)
    {
        float increase = value * (percent / 100);
        return value + increase;
    }
    public static int DecreaseByPercent(int value, float percent)
    {
        float increase = value * (percent / 100);
        return Mathf.RoundToInt(value - increase);
    }
    public static float DecreaseByPercent(float value, float percent)
    {
        float increase = value * (percent / 100);
        return value - increase;
    }

    // 방어율 계산하기  ===========================================================
    public static int CombatScaling(int value)
    {
        // 방어력 100 -> 50% (이후 효율이 감소하는 커브)
        float f_value = value;
        int scale = (int)(f_value / (f_value + Settings.combatScalingConstant) * 100f);

        return scale;
    }

    // 각 웨이브별 웨이브 제한시간 구하기 =========================================
    public static int GetWaveTimer(int waveCount)
    {
        int waveTimer = 0;
        waveTimer = Mathf.Clamp(waveTimer, Settings.waveTimer + (Settings.extraTimePerWave * waveCount), 60);

        return waveTimer;
    }

    // 등급별 색상 리턴하기 ===========================================================
    public static Color GetGradeColor(ELevelUpGrade type)
    {
        Color color = Color.white;

        switch (type)
        {
            case ELevelUpGrade.Normal:
                color = Color.white;
                break;
            case ELevelUpGrade.Rare:
                color = Settings.rare;
                break;
            case ELevelUpGrade.Unique:
                color = Settings.unique;
                break;
            case ELevelUpGrade.Legend:
                color = Settings.legend;
                break;
            default:
                break;
        };

        return color;
    }

    public static Color GetGradeColor(EGrade type, EItemType itemType)
    {
        Color color = Color.white;

        switch (itemType)
        {
            case EItemType.Magnet:
                color = Settings.magnet;
                return color;
            case EItemType.Bomb:
                //color = Settings.rare;
                return color;
        }

        switch (type)
        {
            case EGrade.Normal:
                color = Color.white;
                break;
            case EGrade.Rare:
                color = Settings.rare;
                break;
            case EGrade.Unique:
                color = Settings.unique;
                break;
            case EGrade.Legend:
                color = Settings.legend;
                break;
            default:
                break;
        };

        return color;
    }

    // 스탯별 문자 리턴하기 ===========================================================
    public static string GetStatType(EStatType type)
    {
        string stat = "NULL";

        switch (type)
        {
            case EStatType.Hp:
                stat = "체력";
                break;
            case EStatType.HpRegen:
                stat = "체력 재생";
                break;
            case EStatType.Defense:
                stat = "방어력";
                break;
            case EStatType.BonusDamage:
                stat = "% 데미지";
                break;
            case EStatType.MeleeDamage:
                stat = "근거리 데미지";
                break;
            case EStatType.RangeDamage:
                stat = "원거리 데미지";
                break;
            case EStatType.Speed:
                stat = "속도";
                break;
            case EStatType.Dodge:
                stat = "% 회피";
                break;
            case EStatType.PickUpRange:
                stat = "% 획득 범위";
                break;
            case EStatType.ExpBonus:
                stat = "추가 경험치";
                break;
            default:
                break;
        };

        return stat;
    }

    // 선형 볼륨 스케일을 데시벨로 변환 ====================================================================
    public static float LinearToDecibels(int linear)
    {
        float linearScaleRange = 20f;
        return Mathf.Log10((float)linear / linearScaleRange) * 20f;
    }
}
