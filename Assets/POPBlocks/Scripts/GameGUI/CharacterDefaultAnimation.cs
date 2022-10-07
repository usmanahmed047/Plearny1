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

namespace POPBlocks.Scripts.GameGUI
{
    public class CharacterDefaultAnimation : MonoBehaviour
    {
        [SerializeField] private string triggerName;
        public float delay;
        private void Awake()
        {
            if (delay > 0) StartCoroutine(Delay(delay));
            else if (!string.IsNullOrEmpty(triggerName)) GetComponent<Animator>().SetTrigger(triggerName);
        }

        private IEnumerator Delay(float f)
        {
            yield return new WaitForSeconds(f);
            if (triggerName != null) GetComponent<Animator>().SetTrigger(triggerName);
        }
    }
}