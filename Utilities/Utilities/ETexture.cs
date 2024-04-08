#if UNITY_EDITOR
using UnityEngine;

namespace YNL.Editor.Utilities
{
    public static class ETexture
    {
        public const string TextureAssetPath = EAddress.FolderPath + "Textures";

        public static Texture2D Unity(string name) => $"{TextureAssetPath}/Unity/{name}.png".LoadAsset<Texture2D>();
    }
}
#endif