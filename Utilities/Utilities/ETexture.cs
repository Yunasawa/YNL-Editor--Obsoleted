#if UNITY_EDITOR
using UnityEngine;

namespace YNL.Editor.Utilities
{
    public static class ETexture
    {
        public static Texture2D Unity(string name) => $"Textures/Unity/{name}".LoadAsset<Texture2D>();
    }
}
#endif