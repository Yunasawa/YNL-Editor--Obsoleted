#if UNITY_EDITOR && YNL_UTILITIES
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Extensions.Methods;
using YNL.Extensions.Addons;
using YNL.Editors.Windows.Utilities;

namespace YNL.Editors.UIElements.Flexs
{
    public class FlexComponentIcon : VisualElement
    {
        private const string _styleSheet = "Style Sheets/Elements/Specials/Flex Component Tag/FlexComponentIcon";

        private Image _icon;

        public FlexComponentIcon()
        {
            this.AddStyle(_styleSheet).AddClass("Icon");
        }

        public FlexComponentIcon SetIcon(Texture2D image)
        {
            this.SetBackgroundImage(image);
            return this;
        }
        public FlexComponentIcon SetIcon(string path, MAddressType type)
        {
            if (type == MAddressType.Resources) return SetIcon(path.LoadResource<Texture2D>());
            else if (type == MAddressType.Assets) return SetIcon(path.LoadAsset<Texture2D>());
            return this;
        }

        public FlexComponentIcon SetGlobalColor(Color color)
        {
            this.SetBackgroundImageTintColor(color);
            return this;
        }
    }
}
#endif