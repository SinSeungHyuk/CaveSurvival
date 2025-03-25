using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using static System.Net.WebRequestMethods;

public class AdManager : Singleton<AdManager>
{
    private InterstitialAd interstitialAd; // ���� ���� ��ü

    private string interstitialAdID = "ca-app-pub-3867941567418476/3456717633"; // ���� ����


    protected override void Awake()
    {
        base.Awake();

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            Debug.Log($"���� �ʱ�ȭ!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! - {initStatus.ToString()}");
        });
    }

    public void Start()
    {
        LoadAd();
    }

    private void LoadAd()
    {
        // �̹� �ε�� ����� ����
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        AdRequest adRequest = new AdRequest();

        InterstitialAd.Load(interstitialAdID, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("���� ���ų� ������ �����ϴ�. ���� : " + error);
                    return;
                }

                interstitialAd = ad;
            });
    }

    public void ShowAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            LoadAd(); // ���� ��ε�
        }
    }
}
