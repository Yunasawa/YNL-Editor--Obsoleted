#if UNITY_EDITOR
using System.Linq;
using UnityEditor;

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
            EditorManifest.AddRegistry("YunasawaStudio", "https://package.openupm.com", "com.yunasawa.ynl.utilities");
            EditorManifest.AddDependency("com.yunasawa.ynl.utilities", "1.3.2");
            EditorDefineSymbols.AddSymbols("YNL_UTILITIES");
        }
    }
}
#endif