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
using System.Linq;
using POPBlocks.Scripts.LevelHandle;
using POPBlocks.Scripts.Utils;
using UnityEngine;

namespace POPBlocks.Scripts.Items.BonusItems
{
    public class RocketTrigger : MonoBehaviour
    {
        public Vector2 dir;
        public int wide = 1;
        [HideInInspector] public Vector2Int startPos;
        private bool start;
        public bool[,] items;

        private BoxCollider2D box;
        public bool isVertical;
        [HideInInspector] public BonusRocketEffectData thisBonusParent;

        private BoxCollider2D Box
        {
            get
            {
                if (!box) box = GetComponent<BoxCollider2D>();
                return box;
            }
            set => box = value;
        }

        private void Awake()
        {
            Box = GetComponent<BoxCollider2D>();
            if (!isVertical)
                isVertical = dir.y != 0;
        }

        public void StartTrigger(BonusRocketEffectData _bonusParent)
        {
            if (!(_bonusParent is null))
            {
                thisBonusParent = _bonusParent;
            }
            items = new bool[Field.Instance.size.x, Field.Instance.size.y];
            // items[startPos.x,startPos.y] = true;
            start = true;
            Box.enabled = true;
        }

        public void Finish()
        {
            start = false;
            Box.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject != gameObject)
                CheckItem(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject != gameObject)
                CheckItem(other);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject != gameObject)
                CheckItem(other);
        }

        private void CheckItem(Collider2D other)
        {
            if (!start) return;
            var item = other.GetComponent<Item>();
            if (item)
            {
                var overlapItem = item.GetComponent<OverlapItem>();
                // if (overlapItem != null && overlapItem.ItemUnder != null) return;
            }
            if (item  && item.explodable && !IsItemCollided(item) )
            {
                items[item.pos.x, item.pos.y] = true;
                if (thisBonusParent != null)//for bonuses
                {
                    if (IsValidItem(isVertical ? item.pos.x : item.pos.y, isVertical ? startPos.x : startPos.y))
                    {
                        if (isVertical)
                            Field.Instance.blockColumn[item.pos.x] = gameObject;
                        if (item.bonus?.type == BonusItemTypes.Rocket /*&& !bonusParent.doubleBonus*/)
                        {
                            var collidedRocket = item.GetComponent<Rocket>();
                            if (isVertical && !collidedRocket.IsHorrizontal())
                                collidedRocket.SetHorizontal(true);
                            else if (!isVertical && collidedRocket.IsHorrizontal())
                                collidedRocket.SetHorizontal(false);
                        }

                        item.DestroyItemStart(false, thisBonusParent.delayToDestroy);
                    }
                }   
                else                    //for boosters
                    item.DestroyItemStart();
            }
        }

        private bool IsItemCollided(Item item)
        {
            return items[item.pos.x, item.pos.y];
        }

        bool IsValidItem(int n, int startPos)
        {
            return n <= startPos + (int) Mathf.Sign(n) * wide && n >= startPos - (int) Mathf.Sign(n) * wide;
        }
    }
}