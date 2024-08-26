#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace YNL.Editors.Setups
{
    public class YNLEditorSetup : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            var inPackages = importedAssets.Any(path => path.StartsWith("Packages/")) ||
                deletedAssets.Any(path => path.StartsWith("Packages/")) ||
                movedAssets.Any(path => path.StartsWith("Packages/")) ||
                movedFromAssetPaths.Any(path => path.StartsWith("Packages/"));

            if (inPackages)
            {
                InitializeOnLoad();
            }
        }

        public static void InitializeOnLoad()
        {
#if !YNL_UTILITIES
            Debug.Log($"<color=#FF983D><b>⚠ Caution:</b></color> <color=#fffc54><b>YNL - Editor</b></color> requires <a href=\"https://github.com/Yunasawa/YNL-Utilities\"><b>YNL - Utilities</b></a>");
#else
            EditorDefineSymbols.AddSymbols("YNL_EDITOR");
#endif
        }
    }
}
#endif