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

using AdmobAds;
using GameCurrency;
using POPBlocks.Scripts.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace POPBlocks.Scripts.Popups
{
    public class MainMenu : Popup
    {
        private void Start()
        {
            AdsManager.Instance.ShowBannerAd(true);
            UpdateCashText();
        }

        public void StartGame()
        {
            //if (MonetizationManager.Instance.IsAppOpenAdAvailable)
            //MonetizationManager.Instance.ShowInterstitialAd();
            var gameSettings = Resources.Load<GameSettings>("Settings/GameSettings");
            switch (gameSettings.mapType)
            {
                case MapTypes.NoMap:
                    GameManager.Instance._mapProgressManager.CurrentLevel = GameManager.Instance._mapProgressManager.GetLastLevel();
                    PopupManager.Instance.play1.Show();
                    break;
                case MapTypes.GridLevels:
                    SceneManager.LoadScene("grid");
                    break;
                case MapTypes.ScrollingsMap:
                    /*if(gameSettings.afterLevel >= GameManager.Instance._mapProgressManager.CurrentLevel)
                        SceneManager.LoadScene("game");*/
                    /*else*/
                    SceneManager.LoadScene("map");
                    break;
            }
        }
        public void UpdateCashText()
        {
            FindObjectOfType<CashText>().GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("Cash").ToString();
        }
    }
}