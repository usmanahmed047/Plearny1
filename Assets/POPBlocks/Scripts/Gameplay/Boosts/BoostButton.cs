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
using System.Collections;
using System.Linq;
using DG.Tweening;
using POPBlocks.Scripts.Items;
using POPBlocks.Scripts.Items.BonusItems;
using POPBlocks.Scripts.LevelHandle;
using POPBlocks.Scripts.Popups;
using POPBlocks.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace POPBlocks.Scripts.Boosts
{
    /// <summary>
    /// Boost activation button, used in play_01_menu popup and game scene
    /// </summary>
    public class BoostButton : MonoBehaviour
    {
        //boost type
        public BoostType boostType;
        //GUI element for boost count
        public TextMeshProUGUI guiCounter;
        //count variable
        public int count;
        //boost parameters from settings
        [HideInInspector]public BoostParameters parameters;
        //icon GUI object transform
        public Transform icon;
        //lock object reference
        public GameObject lockObject;
        //Event triggers if count <= 0
        public UnityEvent OnZeroCount;
        //Event triggers if count > 0 updated
        public UnityEvent OnMoreThanZeroCount;
        //Event triggers if boost selected
        public UnityEvent OnSelected;
        //Event triggers if boost unselected
        public UnityEvent OnUnSelected;
        //true if boost selected
        [HideInInspector] public bool selected;

        //count property
        public int Count
        {
            get => count;
            set
            {
                count = value;
                PlayerPrefs.SetInt(boostType.ToString(), count);
                PlayerPrefs.Save();
                UpdateText();
            }
        }

        private void Start()
        {
            parameters = Resources.Load<BoostSettings>("Settings/BoostSettings").boosts.First(i => i.boostType == boostType);
            int instanceLevelNum = GameManager.Instance._mapProgressManager.CurrentLevel;
            if (LevelManager.Instance != null) instanceLevelNum = LevelManager.Instance.levelNum;
            if(instanceLevelNum < parameters.openLevel) Lock();
        }

        private void OnEnable()
        {
            UpdateCount();
        }

        //Update GUI value
        public void UpdateText()
        {
            if (count <= 0) OnZeroCount?.Invoke();
            else OnMoreThanZeroCount?.Invoke();
            guiCounter.text = Count.ToString();
        }

        //spend boost from play popup
        public void SpendBoost()
        {
            if (!selected)
            {
                if (Count > 0)
                {
                    selected = true;
                    OnSelected?.Invoke();
                }
                else
                {
                    var popup = (BoostShop) PopupManager.Instance.boostShop.Show();
                    popup.SetBoost(parameters, icon);
                    popup.Show();
                    popup.OnHide += UpdateCount;
                }
            }
            else
            {
                selected = false;
                OnUnSelected?.Invoke();
            }
        }

        //spend boost from game scene
        public void SpendBoostGame()
        {
            if (lockObject.activeSelf) return;
            if (Count > 0 && !selected)
            {
                selected = true;
                Count--;
                GameManager.Instance.boostTypesGame.Add(boostType);
                OnSelected?.Invoke();
                if (boostType == BoostType.Random) ProceedBoost(boostType, null);
            }
            else if (Count > 0 && selected)
            {
                Count++;
                Unselect();
            }
            else if (Count <= 0)
            {
                var popup = (BoostShop) PopupManager.Instance.boostShop.Show();
                popup.SetBoost(parameters, icon);
                popup.Show();
                popup.OnHide += UpdateCount;
            }
        }

        //unselect boost
        public void Unselect()
        {
            selected = false;
            GameManager.Instance.boostTypes.Clear();
            GameManager.Instance.boostTypesGame.Clear();
            OnUnSelected?.Invoke();
        }

        //lock boost
        public void Lock()
        {
            lockObject.SetActive(true);
            GetComponent<Button>().interactable = false;
            icon.gameObject.SetActive(false);
        }

        //unlock boost
        public void Unlock()
        {
            if (LevelManager.Instance.levelNum < parameters.openLevel) return;
            GetComponent<Button>().interactable = true;
            lockObject.SetActive(false);
            icon.gameObject.SetActive(true);
        }

        //update count value
        private void UpdateCount()
        {
            count = PlayerPrefs.GetInt(boostType.ToString());
            UpdateText();
        }

        //activate boost
        public static void ProceedBoost(BoostType boostType, Item item)
        {
            GameObject o;
            switch (boostType)
            {
                case BoostType.Hammer:
                    o = Instantiate(Resources.Load<GameObject>("Prefabs/booster_hammer_effect"), item.transform.position + new Vector3(1.5f, 0.5f), Quaternion.identity);
                    LevelManager.Instance.StartCoroutine(WaitFor(0.8f, () =>
                    {
                        BoostAnimationFinished(boostType, item);
                        Destroy(o);
                    }));
                    break;
                case BoostType.Torpedo:
                    o = Instantiate(Resources.Load<GameObject>("Prefabs/rocket_booster"), new Vector3(-10, item.transform.position.y-0.3f), Quaternion.Euler(0,0,-90));
                    o.transform.DOMove(new Vector3(8, item.transform.position.y), 1.2f).OnComplete(()=>
                    {
                        // LevelManager.Instance.BoostFinish();
                        Destroy(o);
                    });
                    o.GetComponent<RocketTrigger>().StartTrigger(null);
                    break;
                case BoostType.Pot:
                    o = Instantiate(Resources.Load<GameObject>("Prefabs/pot_booster"), new Vector3(item.transform.position.x, 8), Quaternion.identity);
                    if (LevelManager.Instance.GameState == GameState.Tutorial) o.GetComponent<SpriteRenderer>().sortingLayerName = "Popups";
                    o.transform.DOMove(new Vector3( item.transform.position.x, -10), 15.5f).SetEase(Ease.Linear).SetSpeedBased().OnComplete(()=>
                    {
                        // LevelManager.Instance.BoostFinish();
                        Destroy(o);
                    });
                    o.GetComponent<RocketTrigger>().StartTrigger(null);
                    break;
                case BoostType.Random:
                    SoundBase.Instance.PlayOneShot( SoundBase.Instance.noMatch );
                    var list = Field.Instance.items.Where(i =>!(i is null) && i.colorComponent != null && i.CompareTag("Item") && i.colorComponent.Length > 0);
                    list.ForEachY(i => i.SqueezeAnimation(()=>i.colorComponent[0].RandomizeColor(new int[0]), ()=>LevelManager.Instance.FinishDestroy(), 0.3f));

                    break;
            }

            var boostButtons = FindObjectsOfType<BoostButton>().Where(i => i.selected);
            if(boostButtons.Any())
                boostButtons.First().Unselect();
        }

        static IEnumerator WaitFor(float sec, Action action)
        {
            yield return new WaitForSeconds(sec);
            action?.Invoke();
        }

        //Boost animation finished
        static void BoostAnimationFinished(BoostType boostType, Item item)
        {
            switch (boostType)
            {
                case BoostType.Hammer:
                    if (item.overlapItem)
                    {
                        LevelManager.Instance.moveCounter++;
                        item.overlapItem.DestroyItemStart();
                    }
                    else switch (item.explodable)
                    {
                        case false:
                            LevelManager.Instance.moveCounter++;
                            item.DestroyObstacles(item);
                            break;
                        case true:
                            LevelManager.Instance.moveCounter++;
                            item.DestroyItemStart();
                            break;
                    }
                    // LevelManager.Instance.BoostFinish();

                    break;
                // case BoostType.Torpedo:
                //     // Field.Instance.GetRow(item.pos.y).Where(i => i.explodable).ForEachY(i => i.DestroyItem());
                //     break;
                // case BoostType.Pot:
                //     Field.Instance.GetColumn(item.pos.x).Where(i => i.explodable).ForEachY(i => i.DestroyItem());
                //     // LevelManager.Instance.BoostFinish();
                //
                //     break;
                case BoostType.Random:
                    var list = Field.Instance.items.Where(i => i.CompareTag("Item") && i.colorComponent.Length > 0);
                    foreach (var item1 in list)
                    {
                        item1.colorComponent[0].RandomizeColor(new int[0]);
                    }
                    // LevelManager.Instance.BoostFinish();

                    break;
            }
        }
    }


    public enum BoostType
    {
        Rocket,
        Bomb,
        Pinwheel,
        Hammer,
        Torpedo,
        Pot,
        Random,
        None
    }
}