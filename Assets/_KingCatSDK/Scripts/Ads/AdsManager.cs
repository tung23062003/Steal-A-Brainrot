#if ANALYTICS
using Firebase.Analytics;
#endif
#if ADS
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using GoogleMobileAds.Sample;
#endif

using KingCat.Base.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace KingCat.Base.Ads
{
    public class AdsManager : MonoSingleton<AdsManager>
    {
#if ADS
        [SerializeField] private List<string> _testDeviceIds = new List<string> { "96e23e80653bb28980d3f40beb58915c" };

        //[SerializeField] private AppOpenAdController _openAd;
        [SerializeField] private BannerViewController _banner;
        [SerializeField] private InterstitialAdController _interstitial;
        [SerializeField] private RewardedAdController _rewarded;

        private float _reloadInterval = 10f; // Interval for checking ad load status
        private float _reloadTimer;         // Timer to track time since last check

#if UNITY_EDITOR
        private void OnValidate()
        {
            //_openAd = GetComponentInChildren<AppOpenAdController>();
            _banner = GetComponentInChildren<BannerViewController>();
            _interstitial = GetComponentInChildren<InterstitialAdController>();
            _rewarded = GetComponentInChildren<RewardedAdController>();
        }
#endif


        protected override void OnDestroy()
        {
            base.OnDestroy();
            //AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
            //_openAd.OnAdLoaded.RemoveListener(OnOpenAdLoad);
            _interstitial.OnAdLoaded.RemoveListener(OnInterstitialLoad);
            _rewarded.OnAdLoaded.RemoveListener(OnRewardedLoad);
        }

        //private void OnAppStateChanged(AppState state)
        //{
        //    Debug.Log("App State changed to : " + state);

        //    // If the app is Foregrounded and the ad is available, show it.
        //    if (state == AppState.Foreground)
        //    {
        //        ShowOpenAd();
        //    }
        //}

        public override void Init()
        {
            base.Init();

            //AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
            //_openAd?.OnAdLoaded.AddListener(OnOpenAdLoad);
            _interstitial?.OnAdLoaded.AddListener(OnInterstitialLoad);
            _rewarded?.OnAdLoaded.AddListener(OnRewardedLoad);

            try
            {
                // Configure Mobile Ads settings
                MobileAds.SetiOSAppPauseOnBackground(true);
                MobileAds.RaiseAdEventsOnUnityMainThread = true;

                // Configure request settings
                MobileAds.SetRequestConfiguration(new RequestConfiguration
                {
                    TestDeviceIds = _testDeviceIds
                });

                // Load ads
                LoadAllAds();
            }
            catch (Exception e)
            {
                Debug.LogError($"AdsManager initialization failed: {e.Message}");
            }
        }

        private void Update()
        {
            // Check and reload ads at intervals
            _reloadTimer += Time.deltaTime;
            if (_reloadTimer >= _reloadInterval)
            {
                _reloadTimer = 0f;
                CheckAndReloadAds();
            }
        }

        private void LoadAllAds()
        {
#if SKIP_ADS
            return;
#endif
            //if (_openAd != null) _openAd.LoadAd();
            if (_banner != null) _banner.LoadAd();
            if (_interstitial != null) _interstitial.LoadAd();
            if (_rewarded != null) _rewarded.LoadAd();
        }

        private void CheckAndReloadAds()
        {
            //if (_banner != null && !_banner.IsAdLoaded())
            //{
            //    Debug.Log("Reloading banner ad...");
            //    _banner.LoadAd();
            //}

            if (_interstitial != null && !_interstitial.IsAdLoaded())
            {
                Debug.Log("Reloading interstitial ad...");
                _interstitial.LoadAd();
            }

            if (_rewarded != null && !_rewarded.IsAdLoaded())
            {
                Debug.Log("Reloading rewarded ad...");
                _rewarded.LoadAd();
            }

            //if (_openAd != null && !_openAd.IsAdLoaded())
            //{
            //    Debug.Log("Reloading rewarded ad...");
            //    _openAd.LoadAd();
            //}
        }

        private void OnBannerLoad(bool success)
        {
            Debug.Log("Load Banner: " + success);
        }

        private void OnOpenAdLoad(bool success)
        {
            Debug.Log("Load OpenAd: " + success);
        }

        private void OnInterstitialLoad(bool success)
        {
            Debug.Log("Load Interstitial: " + success);
        }

        private void OnRewardedLoad(bool success)
        {
            Debug.Log("Load Rewarded: " + success);
        }

        public void ShowBanner()
        {
#if SKIP_ADS
            return;
#endif
            if (_banner != null) _banner.ShowAd();
        }

        //public void ShowOpenAd()
        //{
        //    if (_openAd != null && _openAd.IsAdLoaded()) _openAd.ShowAd();
        //}

        public void ShowInterstitial(string from)
        {
#if SKIP_ADS
            return;
#endif

            if (_interstitial != null && _interstitial.IsAdLoaded())
            {
                _interstitial.ShowAd();
#if ANALYTICS
                AnalyticsManager.Instance.LogEvent($"show_interstitial_ads_{from}");
#endif
            }
            else
            {
#if ANALYTICS
                AnalyticsManager.Instance.LogEvent($"fail_interstitial_ads_{from}");
#endif
            }
        }

        public void ShowRewarded(string from, UnityAction<bool> callback)
        {
#if SKIP_ADS
            callback?.Invoke(true);
            return;
#endif
            if (_rewarded != null && _rewarded.IsAdLoaded())
            {
                _rewarded.OnRewarded = callback;
                _rewarded.ShowAd();

#if ANALYTICS
                AnalyticsManager.Instance.LogEvent($"show_rewardedad_ads_{from}");
#endif
            }
            else
            {
#if ANALYTICS
                AnalyticsManager.Instance.LogEvent($"fail_rewardedad_ads_{from}");
#endif
                callback?.Invoke(false);
            }
        }
#endif
    }

}
