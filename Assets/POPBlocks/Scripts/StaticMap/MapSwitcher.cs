using UnityEngine;

namespace POPBlocks.Scripts.StaticMap
{
    // [CreateAssetMenu(fileName = "MapSwitcher", menuName = "MapSwitcher", order = 1)]
    public class MapSwitcher : ScriptableObject
    {
        public bool staticMap;

        public string GetSceneName()
        {
            if (!staticMap) return "game";
            return "gameStatic";
        }
    }
}