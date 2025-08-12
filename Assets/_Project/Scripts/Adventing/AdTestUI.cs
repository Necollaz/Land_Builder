using UnityEngine;
using UnityEngine.UI;

public class AdTestUI : MonoBehaviour
{
    [SerializeField] private Button _interstitialButton;
    [SerializeField] private Button _rewardedButton;
    [SerializeField] private Button _hideBannerButton;

    private void Start()
    {
        _interstitialButton.interactable = false;
        _rewardedButton.interactable = false;

        _interstitialButton.onClick.AddListener(() => MobileAds.Instance.ShowInterstitial(() => Debug.Log("Interstitial закрыт")));
        _rewardedButton.onClick.AddListener(() => MobileAds.Instance.ShowRewarded(() => Debug.Log("Rewarded: выдана награда")));
        _hideBannerButton.onClick.AddListener(() => MobileAds.Instance.HideBanner());
    }

    private void OnEnable()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoaded;
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedLoaded;
    }

    private void OnDestroy()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnInterstitialLoaded;
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardedLoaded;
    }

    private void Update()
    {
        _interstitialButton.interactable = MobileAds.Instance.IsInterstitialReady();
        _rewardedButton.interactable = MobileAds.Instance.IsRewardedReady();
    }
    
    private void OnInterstitialLoaded(string adUnitId, MaxSdkBase.AdInfo info)
    {
        _interstitialButton.interactable = true;
    }
    
    private void OnRewardedLoaded(string adUnitId, MaxSdkBase.AdInfo info)
    {
        _rewardedButton.interactable = true;
    }
}