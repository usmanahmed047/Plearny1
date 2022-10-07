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
using System.Collections;
using POPBlocks.Scripts.Boosts;
using UnityEngine;
using UnityEngine.UI;

namespace POPBlocks.Scripts.Popups
{
    public class BoostTutorialIcon : MonoBehaviour
    {
        public TutorialGamePopup popup;
        public Image image;
        private void Start()
        {
            StartCoroutine(WaitFor());
        }

        private IEnumerator WaitFor()
        {
            yield return new WaitUntil(()=>popup.hightLightedUIObject != null);
            var go = Instantiate(popup.hightLightedUIObject.GetChild(0).GetChild(0), image.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one*1.5f;
            // image.SetNativeSize();
        }
    }
}