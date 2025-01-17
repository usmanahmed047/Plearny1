﻿// // ©2015 - 2021 Candy Smith
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
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace POPBlocks.Scripts.Popups
{
    public class FadeAnim : MonoBehaviour
    {
        private Image fader;

        private void Awake()
        {
            fader = GetComponent<Image>();
        }

        private void OnEnable()
        {
            Popup.OnHidePopup += FadeOut;
            FadeIn();
        }

        private void OnDisable()
        {
            Popup.OnHidePopup -= FadeOut;
        }

        public void FadeIn()
        {
            fader.DOFade(0.9f, 1);
        }

        public void FadeOut(string popupName)
        {
            fader.DOFade(0f, 1);
        }
    }
}