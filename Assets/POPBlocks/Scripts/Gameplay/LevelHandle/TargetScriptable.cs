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
using System.Collections.Generic;
using Malee.List;
using POPBlocks.Scripts.Items;
using UnityEngine;

namespace POPBlocks.Scripts.LevelHandle
{
    [CreateAssetMenu(fileName = "target", menuName = "Blast Match3 Kit/Add target", order = 1)]
    public class TargetScriptable : ScriptableObject
    {
        public bool showInEditor;
        public TargetActions action;
        public bool countFromField;
        [Reorderable(expandByDefault = true)] public Spritelist targetSprites;

        [Header("Optional")] public GameObject prefab;
        // [Reorderable(expandByDefault = true)]
        // public PrefabList prefabs;

        private void OnValidate()
        {
            // if (prefab)
            // {
            //     targetSprites.Clear();
            //     foreach (var texture in prefab.GetComponent<IconEditor>().icon)
            //     {
            //         var spr = new SpriteObject();
            //         var texturePath = UnityEditor.AssetDatabase.GetAssetPath(texture);
            //         //get all generated sprites from a sliced textured.
            //         var sprites = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);
            //         spr.icon = sprites;
            //         spr.uiSprite = true;
            //         targetSprites.Add(spr);
            //     }
            // }
        }
    }

    [Serializable]
    public class PrefabList : ReorderableArray<GameObject>
    {
    }

    [Serializable]
    public class Spritelist : ReorderableArray<SpriteObject>
    {
    }

    [Serializable]
    public struct SpriteObject
    {
        public Sprite icon;

        //uses as separate target in UI 
        public bool uiSprite;
        public int count;
    }

    public enum TargetActions
    {
        Spread,
        ReachBottom,
        Destroy
    }

    [Serializable]
    public class TargetList : ReorderableArray<TargetScriptable>
    {
    }

    [Serializable]
    public class TargetListLevelEditor : ReorderableArray<Target>
    {
    }

    [Serializable]
    public class Target
    {
        public int index;
        public TargetScriptable target;
        public CountArray count;
    }

    [Serializable]
    public class CountArray
    {
        public int[] values = new int[20];
    }

    public class TargetSprite
    {
        public List<Sprite> sprites = new List<Sprite>();
        public int count;
        public bool countFromField;
        public GameObject prefab;
    }
}