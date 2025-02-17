using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;
    [SerializeField] private SpriteRenderer barImage;


    public void EnableHealthBar()
    {
        gameObject.SetActive(true);
    }
    public void DisableHealthBar()
    {
        gameObject.SetActive(false);
    }

    public void SetHealthBar(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);
        healthBar.transform.localScale = new Vector3(ratio, 1f, 1f);
    }

    public async UniTask TakeDamageEffect()
    {
        try
        {
            // 피격시 0.1초 동안 흰색
            barImage.color = Color.white;

            await UniTask.Delay(100);

            barImage.color = Settings.green;
        }
        catch (Exception ex)
        {
            Debug.Log($"TakeDamageEffect - {ex.Message}");
        }
    }
}
