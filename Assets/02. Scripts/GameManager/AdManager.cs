using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using static System.Net.WebRequestMethods;

public class AdManager : Singleton<AdManager>
{
    private InterstitialAd interstitialAd; // 전면 광고 객체

    private string interstitialAdID = "ca-app-pub-3867941567418476/3456717633"; // 전면 광고


    protected override void Awake()
    {
        base.Awake();

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            Debug.Log($"광고 초기화!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! - {initStatus.ToString()}");
        });
    }

    public void Start()
    {
        LoadAd();
    }

    private void LoadAd()
    {
        // 이미 로드된 광고는 해제
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
                    Debug.LogError("광고가 없거나 에러가 났습니다. 에러 : " + error);
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
            LoadAd(); // 광고 재로드
        }
    }
}
