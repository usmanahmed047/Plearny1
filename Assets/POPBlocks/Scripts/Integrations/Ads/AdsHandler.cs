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

using UnityEngine;
using UnityEngine.Events;

namespace POPBlocks.Scripts.Integrations.Ads
{
    public class AdsHandler
    {
        public string id;
        public AdsHandler(string _id)
        {
            id = _id;
        }

        public virtual void ShowAds()
        {
            Debug.Log("Trying to show " + id);
        }

        public virtual bool IsAvailable()
        {
            return false;
        }

        public virtual void ShowRewardedAds()
        {
            SoundBase.Instance.MuteMusic(true);
            // Debug.Log("Trying to show " + id);
        }

        public delegate void RewEvent();
        public event RewEvent OnRewarded;
        public event RewEvent OnRewardedFail;

        public void RewardedShown()
        {
            SoundBase.Instance.MuteMusic(false);
            OnRewarded?.Invoke();
        }

        public void RewardedFail()
        {
            OnRewardedFail?.Invoke();
        }
        public UnityEvent act;
    }
}