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

using UnityEngine.Advertisements;

namespace POPBlocks.Scripts.Integrations.Ads
{
#if UNITY_ADS

    public class UnityAdsHandler : AdsHandler, IUnityAdsListener
    {
        public UnityAdsHandler(string _id) : base(_id)
        {
  #if UNITY_ADS
            Advertisement.AddListener(this);
            Advertisement.Initialize(_id);
#endif
        }
        public override void ShowAds()
        {
#if UNITY_ADS
            Advertisement.Show("video");
            base.ShowAds();
#endif
        }

        public override bool IsAvailable()
        {
#if UNITY_ADS
            return Advertisement.IsReady("rewardedVideo");
#else
            return false;
#endif
        }

        public override void ShowRewardedAds()
        {
#if UNITY_ADS
            if (IsAvailable())
                Advertisement.Show("rewardedVideo");
            base.ShowRewardedAds();
#endif
        }

        public void OnUnityAdsReady(string placementId)
        {
        }

        public void OnUnityAdsDidError(string message)
        {
        }

        public void OnUnityAdsDidStart(string placementId)
        {
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            if (placementId == "rewardedVideo" && showResult == ShowResult.Finished)
            {
                RewardedShown();
            }
        }
    }
    #endif
}