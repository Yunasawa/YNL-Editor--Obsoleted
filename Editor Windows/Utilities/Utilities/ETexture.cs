#if UNITY_EDITOR
using UnityEngine;
using YNL.Editors.Windows.Utilities;

namespace YNL.Editors.Windows.Utilities
{
    public static class ETexture
    {
        public static Texture2D Unity(string name) => $"Textures/Unity/{name}".ELoadAsset<Texture2D>();
    }
}
#endif