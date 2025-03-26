using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;


public class AdManager : Singleton<AdManager>
{
    private readonly string testInterstitialAdID = "ca-app-pub-3940256099942544/1033173712"; // 테스트 광고
    private readonly string interstitialAdID = "ca-app-pub-3867941567418476/3456717633"; // AdMob에 등록한 광고

    private InterstitialAd interstitialAd;


    protected override void Awake()
    {
        base.Awake();

        MobileAds.Initialize((InitializationStatus status) =>
                Debug.Log("MoblieAds 초기화 완료"));
    }

    private void Start()
    {
        LoadInterstitialAd();
    }

    public void LoadInterstitialAd()
    {
        // 이미 광고가 있다면 광고를 해제
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        var adRequest = new AdRequest(); // 광고 요청

        // id를 넣어서 광고 로드
        InterstitialAd.Load(interstitialAdID, adRequest, (InterstitialAd ad, LoadAdError error) => {
            if (error != null || ad == null)
            {
                Debug.LogError($"광고를 찾을 수 없음 : {error}");
                return;
            }

            interstitialAd = ad;
            RegisterAdEvent(interstitialAd); // 광고 이벤트 등록
        });
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show(); // Show() 함수로 광고 재생
        }
        else
        {
            Debug.LogError("로드된 광고가 없음");
        }
    }


    private void RegisterAdEvent(InterstitialAd ad)
    {
        // 광고를 닫으면서 다시 새로운 광고 로드
        // 게임이 끝날때 재생되는 광고 => 광고를 닫고 메인메뉴로 이동
        ad.OnAdFullScreenContentClosed += () =>
        {
            LoadInterstitialAd();

            AddressableManager.Instance.ReleaseGroup("Stage1");
            LoadingSceneManager.LoadScene("MainMenuScene", "NULL", ESceneType.MainMenu);
        };

        // 예외처리 이벤트
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("광고 띄우기 실패 : " + error);
        };
    }

}
