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
#if ADMOB
using GoogleMobileAds.Api;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace POPBlocks.Scripts.Integrations.Ads
{
    public class AdmobAdsHandler: AdsHandler
    {
        private AdmobController admobController;

        public AdmobAdsHandler(string _id, AdmobController controller) : base(_id)
        {
            admobController = controller;
            #if ADMOB
            admobController.RequestAndLoadInterstitialAd();
            admobController.RequestAndLoadRewardedAd();
            admobController.OnUserEarnedRewardEvent.AddListener(RewardedShown);
            admobController.OnAdFailedToLoadEvent.AddListener(RewardedFail);
            admobController.OnAdClosedEvent.AddListener(RewardedFail);
            admobController.OnAdFailedToShowEvent.AddListener(RewardedFail);
            #endif
        }

#if ADMOB
        public override void ShowAds()
        {
            admobController?.ShowInterstitialAd();
            admobController.RequestAndLoadInterstitialAd();
            base.ShowAds();
        }

        
        public override bool IsAvailable()
        {
            var isAvailable = admobController.interstitialAd.IsLoaded();
            Debug.Log("ADMOB interstitial loaded " + isAvailable);
            return isAvailable;
        }

        public override void ShowRewardedAds()
        {
            admobController.ShowRewardedAd();
            admobController.RequestAndLoadRewardedAd();
            base.ShowRewardedAds();
        }

        private InterstitialAd interstitial;
        private RewardedAd rewardedAd;
        private AdRequest request;

        private void RequestInterstitial(AdRequest adRequest)
        {
            admobController?.RequestAndLoadInterstitialAd();
           
        }
     
#endif
    }
}