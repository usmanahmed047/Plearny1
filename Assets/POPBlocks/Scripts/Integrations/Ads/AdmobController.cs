// // ©2015 - 2021 Candy Smith
// // All rights reserved
// // Redistribution of this software is strictly not allowed.
// // Copy of this software can be obtained from unity asset store only.
// 
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// // THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
#if ADMOB
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace POPBlocks.Scripts.Integrations.Ads
{
    public class AdmobController : MonoBehaviour
    {
        public AdItem adsSettingsAd;
        #if ADMOB
        
        private AppOpenAd appOpenAd;
        private BannerView bannerView;
        public  InterstitialAd interstitialAd;
        public RewardedAd rewardedAd;
        private RewardedInterstitialAd rewardedInterstitialAd;

        public UnityEvent OnAdLoadedEvent = new UnityEvent();
        public UnityEvent OnAdFailedToLoadEvent = new UnityEvent();
        public UnityEvent OnAdOpeningEvent = new UnityEvent();
        public UnityEvent OnAdFailedToShowEvent = new UnityEvent();
        public UnityEvent OnUserEarnedRewardEvent = new UnityEvent();
        public UnityEvent OnAdClosedEvent = new UnityEvent();
        public UnityEvent OnAdLeavingApplicationEvent = new UnityEvent();

        #region UNITY MONOBEHAVIOR METHODS

        public void Start()
        {
            MobileAds.SetiOSAppPauseOnBackground(true);

            List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };

            // Add some test device IDs (replace with your own device IDs).
#if UNITY_IPHONE
        deviceIds.Add("96e23e80653bb28980d3f40beb58915c");
#elif UNITY_ANDROID
            deviceIds.Add("BD7AD3D5D206E54FF4E0F382C3ECB01B");
#endif

            // Configure TagForChildDirectedTreatment and test device IDs.
            RequestConfiguration requestConfiguration =
                new RequestConfiguration.Builder()
                    .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
                    .SetTestDeviceIds(deviceIds).build();
            MobileAds.SetRequestConfiguration(requestConfiguration);

            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(HandleInitCompleteAction);
        }

        private void HandleInitCompleteAction(InitializationStatus initstatus)
        {
            // Callbacks from GoogleMobileAds are not guaranteed to be called on
            // main thread.
            // In this example we use MobileAdsEventExecutor to schedule these calls on
            // the next Update() loop.
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                Debug.Log("Initialization complete");
                //RequestBannerAd();
            });
        }

        #endregion 

        #region HELPER METHODS

        private AdRequest CreateAdRequest()
        {
            return new AdRequest.Builder()
                // .AddTestDevice(AdRequest.TestDeviceSimulator)
                // .AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
                // .AddKeyword("unity-admob-sample")
                // .TagForChildDirectedTreatment(false)
                // .AddExtra("color_bg", "9B30FF")
                .Build();
        }

        #endregion

        #region BANNER ADS

        public void RequestBannerAd()
        {
            // Clean up banner before reusing
            if (bannerView != null)
            {
                bannerView.Destroy();
            }

            var adUnitId = adsSettingsAd.adsId.First(i => i.type == AdType.Banner).id;
            
            // Create a 320x50 banner at top of the screen
            bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);

            // Add Event Handlers
            bannerView.OnAdLoaded += (sender, args) => OnAdLoadedEvent?.Invoke();
            bannerView.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent?.Invoke();
            bannerView.OnAdOpening += (sender, args) => OnAdOpeningEvent?.Invoke();
            bannerView.OnAdClosed += (sender, args) => OnAdClosedEvent?.Invoke();
            // bannerView.OnAdLeavingApplication += (sender, args) => OnAdLeavingApplicationEvent?.Invoke();

            // Load a banner ad
            bannerView.LoadAd(CreateAdRequest());
        }

        public void DestroyBannerAd()
        {
            if (bannerView != null)
            {
                bannerView.Destroy();
            }
        }

        #endregion

        #region INTERSTITIAL ADS

        public void RequestAndLoadInterstitialAd()
        {
            var adUnitId = adsSettingsAd.adsId.First(i => i.type == AdType.Interstitial).id;
            
            // Clean up interstitial before using it
            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
            }

            interstitialAd = new InterstitialAd(adUnitId);

            // Add Event Handlers
            interstitialAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent?.Invoke();
            interstitialAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent?.Invoke();
            interstitialAd.OnAdOpening += (sender, args) => OnAdOpeningEvent?.Invoke();
            interstitialAd.OnAdClosed += (sender, args) => OnAdClosedEvent?.Invoke();
           
            // interstitialAd.OnAdLeavingApplication += (sender, args) => OnAdLeavingApplicationEvent?.Invoke();

            // Load an interstitial ad
            interstitialAd.LoadAd(CreateAdRequest());
        }

        public void ShowInterstitialAd()
        {
            if (interstitialAd.IsLoaded())
            {
                interstitialAd.Show();
            }
            else
            {
                Debug.Log("Interstitial ad is not ready yet");
            }
        }

        public void DestroyInterstitialAd()
        {
            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
            }
        }
        #endregion

        #region REWARDED ADS

        public void RequestAndLoadRewardedAd()
        {
            Debug.Log("Requesting Rewarded Ad.");
            
            if (!adsSettingsAd.adsId.Any(i=>i.type==AdType.RewardedAd)) Debug.LogError("Setup admob rewarded ads!");
            var adUnitId = adsSettingsAd.adsId.First(i => i.type == AdType.RewardedAd).id;
            
            //rewardedAd = new RewardedAd(adsSettingsAd.adsId.First(i=>i.type==AdType.RewardedAd).id);
            
            // create new rewarded ad instance
            rewardedAd = new RewardedAd(adUnitId);

            // Add Event Handlers
            rewardedAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent?.Invoke();
            rewardedAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent?.Invoke();
            rewardedAd.OnAdOpening += (sender, args) => OnAdOpeningEvent?.Invoke();
            rewardedAd.OnAdFailedToShow += (sender, args) => OnAdFailedToShowEvent?.Invoke();
            rewardedAd.OnAdClosed += (sender, args) => OnAdClosedEvent?.Invoke();
            rewardedAd.OnUserEarnedReward += (sender, args) => OnUserEarnedRewardEvent?.Invoke();

            // Create empty ad request
            rewardedAd.LoadAd(CreateAdRequest());
        }

        public void ShowRewardedAd()
        {
            if (rewardedAd != null)
            {
                rewardedAd.Show();
            }
            else
            {
                Debug.Log("Rewarded ad is not ready yet.");
            }
        }

        public void RequestAndLoadRewardedInterstitialAd()
        {
            var adUnitId = adsSettingsAd.adsId.First(i=>i.type==AdType.RewardedAd).id;
            
            // Create an interstitial.
            RewardedInterstitialAd.LoadAd(adUnitId, CreateAdRequest(), (rewardedInterstitialAd, error) =>
            {
                if (error != null)
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() => {
                    });
                    return;
                }

                this.rewardedInterstitialAd = rewardedInterstitialAd;
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                });
                // Register for ad events.
                this.rewardedInterstitialAd.OnAdDidPresentFullScreenContent += (sender, args) =>
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() => {
                    });
                };
                this.rewardedInterstitialAd.OnAdDidDismissFullScreenContent += (sender, args) =>
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() => {
                    });
                    this.rewardedInterstitialAd = null;
                };
                this.rewardedInterstitialAd.OnAdFailedToPresentFullScreenContent += (sender, args) =>
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() => {
                    });
                    this.rewardedInterstitialAd = null;
                };
            });
        }

        public void ShowRewardedInterstitialAd()
        {
            if (rewardedInterstitialAd != null)
            {
                rewardedInterstitialAd.Show((reward) => {
                    MobileAdsEventExecutor.ExecuteInUpdate(() => {
                    });
                });
            }
            else
            {
            }
        }

        #endregion
        #endif
    }
}