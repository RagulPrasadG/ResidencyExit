using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdService
{
    private GameDataScriptableObject gameDataSO;


#if UNITY_EDITOR || Dev
    private string bannerAdId = "ca-app-pub-3940256099942544/6300978111";
    private string rewardedAdId = "ca-app-pub-3940256099942544/5224354917";
    private string interstitialAdId = "ca-app-pub-3940256099942544/1033173712";

#else
    private string bannerAdId = "ca-app-pub-8448169234004107/9312616990";
    private string rewardedAdId = "ca-app-pub-8448169234004107/9639920052";
    private string interstitialAdId = "ca-app-pub-8448169234004107/1523247575";

#endif

    private BannerView _bannerView;
    private RewardedAd rewardedAd;
    private InterstitialAd interstitialAd;

    public AdService(GameDataScriptableObject gameDataSO)
    {
        this.gameDataSO = gameDataSO;
        InitializeAdMob();
        LoadRewardedAd();
    }

    public void InitializeAdMob()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus => { Debug.Log("AdMob is initialized!"); });

    }

    public void CreateBannerAd()
    {
        Debug.Log("Creating banner view");

        if (_bannerView != null)
        {
            DestroyBannerAd();
        }

        _bannerView = new BannerView(bannerAdId, AdSize.Banner, AdPosition.Bottom);

    }

    public void DestroyBannerAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner ad.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    public void LoadBannerAd()
    {

        if (_bannerView == null)
        {
            CreateBannerAd();
        }

        var adRequest = new AdRequest();


        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);

        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + _bannerView.GetResponseInfo());
        };

        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
    }

    public void LoadRewardedAd()
    {

        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        var adRequest = new AdRequest();

        RewardedAd.Load(rewardedAdId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());
 
                rewardedAd = ad;
            });
     
    }

    public void ShowRewardedAd(int rewardAmount = 200)
    {
        RegisterRewardedAdReloadHandler(rewardedAd);
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                gameDataSO.coinAmount += rewardAmount;
                GameService.Instance.mainMenuUIService.SetCoinText();
            });
        }
    }



    public void LoadInterstitialAd()
    {

        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        var adRequest = new AdRequest();

        InterstitialAd.Load(interstitialAdId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
            });
    }

    public void ShowInterstitialAd()
    {
        RegisterInterstitialReloadHandler(interstitialAd);
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }

    }


    private void RegisterRewardedAdReloadHandler(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");
            LoadRewardedAd();
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            LoadRewardedAd();
        };
    }

    private void RegisterInterstitialReloadHandler(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial Ad full screen content closed.");

            LoadInterstitialAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            LoadInterstitialAd();
        };
    }
}
