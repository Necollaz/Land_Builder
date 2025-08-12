using System;
using System.Collections;
using UnityEngine;
using Firebase.Analytics;

public class MobileAds : MonoBehaviour
{
    public static MobileAds Instance { get; private set; }

    [Header("Ad Unit IDs (из AppLovin Dashboard)")]
    [SerializeField, Tooltip("ID межстраничного объявления (Interstitial)")] private string _interstitialAdUnitId = "INTERSTITIAL_AD_UNIT_ID";
    [SerializeField, Tooltip("ID вознаграждаемого видео (Rewarded)")] private string _rewardedAdUnitId = "REWARDED_AD_UNIT_ID";
    [SerializeField, Tooltip("ID баннера (Banner)")] private string _bannerAdUnitId = "BANNER_AD_UNIT_ID";
    [SerializeField, Tooltip("Интервал в секундах между авто-перезагрузками вознаграждаемого видео")] private float _rewardedReloadInterval = 60f;

    private bool _sdkInitialized;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            MaxSdk.InitializeSdk();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitialized;
        
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoaded;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailed;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHidden;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialOpened;

        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedLoaded;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedFailed;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedHidden;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedOpened;

        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
    }

    private void OnDisable()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent -= OnSdkInitialized;

        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnInterstitialLoaded;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnInterstitialFailed;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnInterstitialHidden;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent -= OnInterstitialOpened;

        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardedLoaded;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnRewardedFailed;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnRewardedHidden;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnRewardedOpened;

        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;
    }

    public void ShowInterstitial(Action onComplete = null)
    {
        if (Instance == null || !Instance._sdkInitialized || !MaxSdk.IsInterstitialReady(Instance._interstitialAdUnitId))
            return;

        FirebaseAnalytics.LogEvent("Interstitial_show");
        
        void Callback(string _, MaxSdkBase.AdInfo info)
        {
            onComplete?.Invoke();
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= Callback;
        }

        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += Callback;
        
        MaxSdk.ShowInterstitial(Instance._interstitialAdUnitId);
    }

    public void ShowRewarded(Action onRewarded = null)
    {
        if (Instance == null || !Instance._sdkInitialized || !MaxSdk.IsRewardedAdReady(Instance._rewardedAdUnitId))
            return;

        FirebaseAnalytics.LogEvent("Rewarded_show");
        
        void Callback(string _, MaxSdkBase.AdInfo info)
        {
            onRewarded?.Invoke();
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= Callback;

            MaxSdk.LoadRewardedAd(Instance._rewardedAdUnitId);
        }

        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += Callback;
        
        MaxSdk.ShowRewardedAd(Instance._rewardedAdUnitId);
    }

    public void HideBanner()
    {
        if (Instance != null && Instance._sdkInitialized)
            MaxSdk.HideBanner(Instance._bannerAdUnitId);
    }

    public bool IsRewardedReady()
    {
        return Instance != null && MaxSdk.IsRewardedAdReady(Instance._rewardedAdUnitId);
    }

    public bool IsInterstitialReady()
    {
        return Instance != null && MaxSdk.IsInterstitialReady(Instance._interstitialAdUnitId);
    }
    
    private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        LogFirebaseImpression(adInfo);
    }
    
    private void LogFirebaseImpression(MaxSdkBase.AdInfo adInfo)
    {
        Parameter[] parameters = new Parameter[]
        {
            new Parameter("ad_platform", "applovin_max_sdk"),
            new Parameter("ad_source", adInfo.NetworkName),
            new Parameter("ad_unit_name", adInfo.AdUnitIdentifier),
            new Parameter("ad_format", adInfo.AdFormat.ToString()),
            new Parameter("currency", "USD"),
            new Parameter("value", adInfo.Revenue)
        };
        
        FirebaseAnalytics.LogEvent("ad_impression", parameters);
    }
    
    private void OnSdkInitialized(MaxSdkBase.SdkConfiguration config)
    {
        _sdkInitialized = true;

        MaxSdk.LoadInterstitial(_interstitialAdUnitId);
        MaxSdk.LoadRewardedAd(_rewardedAdUnitId);
        CreateAndShowBanner();
        
        StartCoroutine(AutoReloadRewarded());
    }

    private void OnInterstitialLoaded(string adUnitId, MaxSdkBase.AdInfo info) { }

    private void OnInterstitialFailed(string adUnitId, MaxSdkBase.ErrorInfo info)
    {
        MaxSdk.LoadInterstitial(_interstitialAdUnitId);
    }

    private void OnInterstitialOpened(string adUnitId, MaxSdkBase.AdInfo info)
    {
        Time.timeScale = 0;
        AudioListener.volume = 0;
    }
    
    private void OnInterstitialHidden(string adUnitId, MaxSdkBase.AdInfo info)
    {
        Time.timeScale = 1;
        AudioListener.volume = 1;
        
        MaxSdk.LoadInterstitial(_interstitialAdUnitId);
    }

    private void OnRewardedLoaded(string adUnitId, MaxSdkBase.AdInfo info) { }

    private void OnRewardedFailed(string adUnitId, MaxSdkBase.ErrorInfo info)
    {
        MaxSdk.LoadRewardedAd(_rewardedAdUnitId);
    }

    private void OnRewardedOpened(string adUnitId, MaxSdkBase.AdInfo info)
    {
        Time.timeScale = 0;
        AudioListener.volume = 0;
    }
    
    private void OnRewardedHidden(string adUnitId, MaxSdkBase.AdInfo info)
    {
        Time.timeScale = 1;
        AudioListener.volume = 1;
        
        MaxSdk.LoadRewardedAd(_rewardedAdUnitId);
    }
    
    private void CreateAndShowBanner()
    {
        MaxSdk.CreateBanner(_bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerBackgroundColor(_bannerAdUnitId, Color.clear);
        MaxSdk.ShowBanner(_bannerAdUnitId);
        
        FirebaseAnalytics.LogEvent("Banner_show");
    }
    
    private IEnumerator AutoReloadRewarded()
    {
        while (_sdkInitialized)
        {
            yield return new WaitForSeconds(_rewardedReloadInterval);
            
            MaxSdk.LoadRewardedAd(_rewardedAdUnitId);
        }
    }
}