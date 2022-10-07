using UnityEngine;
using UnityEngine.UI;

namespace POPBlocks.Scripts.MapScripts.StaticMap
{
    /// <summary>
    /// Background selector. Select different level background for every 20 levels
    /// </summary>
    public class BackgroundsForStaticMap : MonoBehaviour
    {
        public Sprite[] pictures;

        // Use this for initialization
        void OnEnable ()
        {
            {
                var backgroundSpriteNum = (int) (LevelManager.Instance.levelNum / 20f - 0.01f);
                if(pictures.Length > backgroundSpriteNum)
                    GetComponent<Image>().sprite = pictures[backgroundSpriteNum];
            }


        }


    }
}