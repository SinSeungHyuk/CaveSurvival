using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


// �������� ���ٰ����� ���� ��� static Ŭ����
public static class UtilitieHelper
{
    // ���ͷκ��� ���� ���ϱ�  ===========================================================
    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x); // Atan2(y,x) ���� ���ϱ�
        float degrees = radians * Mathf.Rad2Deg;         // ���� ��׸��� ��ȯ

        return degrees;
    }

    // �����κ��� ���⺤�� ���ϱ�  ===========================================================
    public static Vector3 GetDirectionVectorFromAngle(float angle)
    {
        Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
        return direction;
    }

    // Ȯ�� ����ϱ�  ===========================================================
    public static bool isSuccess(int percent)
    {
        int chance = Random.Range(1, 101);
        return percent >= chance; // �����ϸ� true
    }
    public static bool isSuccess(float percent)
    {
        int chance = Random.Range(1, 101);
        return percent >= chance; // �����ϸ� true
    }

    // �ۼ�Ʈ % ����ϱ�  ===========================================================
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

    // ����� ����ϱ�  ===========================================================
    public static int CombatScaling(int value)
    {
        // ���� 100 -> 50% (���� ȿ���� �����ϴ� Ŀ��)
        float f_value = value;
        int scale = (int)(f_value / (f_value + Settings.combatScalingConstant) * 100f);

        return scale;
    }

    // ��޺� ���� �����ϱ� ===========================================================
    public static Color GetGradeColor(EGrade type)
    {
        Color color = Color.white;

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

    // ���� ���� �������� ���ú��� ��ȯ ====================================================================
    public static float LinearToDecibels(int linear)
    {
        float linearScaleRange = 20f;
        return Mathf.Log10((float)linear / linearScaleRange) * 20f;
    }
}
