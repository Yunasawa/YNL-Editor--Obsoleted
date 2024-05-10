#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using YNL.Extensions.Methods;

namespace YNL.Editors
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

        //[InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            MDebug.Notify("InitializeOnLoad");
        }
    }
}
#endif