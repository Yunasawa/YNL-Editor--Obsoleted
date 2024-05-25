#if UNITY_EDITOR
#if YNL_UTILITIES
using UnityEngine;
using YNL.Extensions.Methods;

namespace YNL.Editors.Windows.Utilities
{
    public static class ETexture
    {
        public static Texture2D Unity(string name) => $"Textures/Unity/{name}".LoadAsset<Texture2D>();
    }
}
#endif
#endif