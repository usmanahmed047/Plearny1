using System.IO;
using POPBlocks.Scripts.Utils;
using UnityEditor;

namespace POPBlocks.Scripts.Editor
{
    public class PostImporting : AssetPostprocessor
    {

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            CheckDefines("Assets/GoogleMobileAds", "ADMOB");
            CheckDefines("Assets/Chartboost", "CHARTBOOST_ADS");
            CheckDefines("Assets/FacebookSDK", "FACEBOOK");
            CheckDefines("Assets/PlayFabSDK", "PLAYFAB");
            CheckDefines("Assets/GameSparks", "GAMESPARKS");
            CheckDefines("Assets/Appodeal", "APPODEAL");
            CheckDefines("Assets/GetSocial", "USE_GETSOCIAL_UI");
        }

        static void CheckDefines(string path, string symbols)
        {
            if (Directory.Exists(path))
            {
                DefineSymbolsUtils.AddSymbol(symbols);
            }
            else
            {
                DefineSymbolsUtils.DeleteSymbol(symbols);
            }
        }
    }
}