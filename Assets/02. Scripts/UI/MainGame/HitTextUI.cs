using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class HitTextUI : MonoBehaviour
{
    private TextMeshPro txtDamage;
    private Transform trans;


    private void Awake()
    {
        txtDamage = GetComponent<TextMeshPro>();
        trans = GetComponent<Transform>();
    }

    public void InitializeHitText(int damageAmount, EHitType hitType = EHitType.Normal)
    {
        switch (hitType)
        {
            case EHitType.Dodge:
                txtDamage.text = "È¸ÇÇ";
                HitTextMove(Color.white, 4f, 0.6f);
                break;
            case EHitType.Normal:
                txtDamage.text = damageAmount.ToString();
                HitTextMove(Color.white, 4f, 0.6f);
                break;
            case EHitType.Critical:
                txtDamage.text = damageAmount.ToString();
                HitTextMove(Settings.critical, 5f, 0.8f);
                break;
            case EHitType.PlayerHit:
                txtDamage.text = "-"+damageAmount.ToString();
                HitTextMove(Settings.legend, 4f, 0.6f);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void HitTextMove(Color color, float fontSize, float yPos)
    {
        txtDamage.color = color;
        txtDamage.fontSize = fontSize;

        trans.DOMoveY(yPos, yPos).SetEase(Ease.InOutQuad).SetRelative()
            .OnComplete(() => ObjectPoolManager.Instance.Release(gameObject,EPool.HitText));
    }
}
