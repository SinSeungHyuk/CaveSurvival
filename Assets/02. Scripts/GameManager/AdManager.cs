using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;


public class AdManager : Singleton<AdManager>
{
    private readonly string testInterstitialAdID = "ca-app-pub-3940256099942544/1033173712"; // �׽�Ʈ ����
    private readonly string interstitialAdID = "ca-app-pub-3867941567418476/3456717633"; // AdMob�� ����� ����

    private InterstitialAd interstitialAd;


    protected override void Awake()
    {
        base.Awake();

        MobileAds.Initialize((InitializationStatus status) =>
                Debug.Log("MoblieAds �ʱ�ȭ �Ϸ�"));
    }

    private void Start()
    {
        LoadInterstitialAd();
    }

    public void LoadInterstitialAd()
    {
        // �̹� ���� �ִٸ� ���� ����
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        var adRequest = new AdRequest(); // ���� ��û

        // id�� �־ ���� �ε�
        InterstitialAd.Load(interstitialAdID, adRequest, (InterstitialAd ad, LoadAdError error) => {
            if (error != null || ad == null)
            {
                Debug.LogError($"���� ã�� �� ���� : {error}");
                return;
            }

            interstitialAd = ad;
            RegisterAdEvent(interstitialAd); // ���� �̺�Ʈ ���
        });
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show(); // Show() �Լ��� ���� ���
        }
        else
        {
            Debug.LogError("�ε�� ���� ����");
        }
    }


    private void RegisterAdEvent(InterstitialAd ad)
    {
        // ���� �����鼭 �ٽ� ���ο� ���� �ε�
        // ������ ������ ����Ǵ� ���� => ���� �ݰ� ���θ޴��� �̵�
        ad.OnAdFullScreenContentClosed += () =>
        {
            LoadInterstitialAd();

            AddressableManager.Instance.ReleaseGroup("Stage1");
            LoadingSceneManager.LoadScene("MainMenuScene", "NULL", ESceneType.MainMenu);
        };

        // ����ó�� �̺�Ʈ
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("���� ���� ���� : " + error);
        };
    }

}
