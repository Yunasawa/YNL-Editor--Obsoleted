#if UNITY_EDITOR
using UnityEngine;
using YNL.Editor.Extension;

namespace YNL.Editor.Utility
{
    public static class ETexture
    {
        public static Texture2D Unity(string name) => $"Textures/Unity/{name}".ELoadAsset<Texture2D>();
    }
}
#endif