#if UNITY_EDITOR 
#if YNL_UTILITIES
using UnityEngine.TextCore.Text;
using YNL.Extensions.Methods;

namespace YNL.Editors.Utilities
{
    public class EditorFontAsset
    {
        public static FontAsset Somatic => "Fonts/Font Assets/Somatic-Rounded SDF".LoadResource<FontAsset>();
    }
}
#endif
#endif