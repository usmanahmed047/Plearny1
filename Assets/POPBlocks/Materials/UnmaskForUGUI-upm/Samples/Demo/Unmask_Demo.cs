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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Coffee.UIExtensions.Demos
{
	public class Unmask_Demo : MonoBehaviour
	{
		[SerializeField] Button target;
		[SerializeField] Unmask unmask;
		[SerializeField] Graphic transition;
		[SerializeField] Image transitionImage;
		[SerializeField] Sprite unity_chan;
		[SerializeField] Sprite unity_frame;

		public void AutoFitToButton(bool flag)
		{
			unmask.fitOnLateUpdate = flag;
		}

		public void SetTransitionColor(bool flag)
		{
			transition.color = flag ? Color.white : Color.black;
		}

		public void SetTransitionImage(bool flag)
		{
			transitionImage.sprite = flag ? unity_chan : unity_frame;
			transitionImage.SetNativeSize();
			var size = transitionImage.rectTransform.rect.size;
			transitionImage.rectTransform.sizeDelta = new Vector2(150, size.y / size.x * 150);
		}
	}
}