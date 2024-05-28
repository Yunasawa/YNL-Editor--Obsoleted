#if UNITY_EDITOR 
#if YNL_UTILITIES
using UnityEngine;
using YNL.Extensions.Methods;

namespace YNL.Editors.Utilities
{
    public class EditorIcon
    {
        public static Texture2D Visible => "Textures/Icons/Visible".LoadResource<Texture2D>();
        public static Texture2D Invisible => "Textures/Icons/Invisible".LoadResource<Texture2D>();
    }
}
#endif
#endif