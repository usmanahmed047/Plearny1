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

using POPBlocks.Scripts.Popups;
using TMPro;
using UnityEngine;

namespace POPBlocks.Scripts.Boosts
{
    public class BoostShop : Popup
    {
        public Transform iconTransform;
        public TextMeshProUGUI cost;
        public TextMeshProUGUI boostDescription;
        public TextMeshProUGUI title;
        private BoostParameters parameters;

        public void SetBoost(BoostParameters param, Transform icon)
        {
            parameters = param;
            var g = Instantiate(icon.gameObject, iconTransform);
            g.transform.localScale = Vector3.one;
            cost.text = parameters.cost.ToString();
            boostDescription.text = parameters.description;
            title.text = parameters.boostType.ToString();
        }
        public void BuyBoost()
        {
            if (GameManager.Instance.Purchasing(parameters.cost))
            {
                PlayerPrefs.SetInt(parameters.boostType.ToString(), parameters.purchasingAmount);
                PlayerPrefs.Save();
                Hide();
            }
        }
    }
}