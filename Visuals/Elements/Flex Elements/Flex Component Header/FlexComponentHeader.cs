#if UNITY_EDITOR && YNL_UTILITIES
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Extensions.Methods;
using YNL.Extensions.Addons;
using YNL.Editors.Extensions;

namespace YNL.Editors.UIElements.Flex
{
    public class FlexComponentHeader : VisualElement
    {
        private const string _styleSheet = "Style Sheets/Elements/Specials/Flex Component Tag/FlexComponentHeader";

        private Color _globalColor = "#AFAFAF".ToColor();

        private FlexComponentTitle _flexTitle;
        private FlexComponentIcon _flexIcon;

        public FlexComponentHeader()
        {
            this.AddStyle(_styleSheet, ESheet.Font).AddClass("Main");

            _flexTitle = new FlexComponentTitle();
            _flexIcon = new FlexComponentIcon();
        }

        #region Flex Component Header
        public FlexComponentHeader SetGlobalColor(string color) => SetGlobalColor(color.ToColor());
        public FlexComponentHeader SetGlobalColor(Color color)
        {
            _globalColor = color;

            _flexTitle.SetGlobalColor(_globalColor);
            _flexIcon.SetGlobalColor(_globalColor);

            return this;
        }

        public FlexComponentHeader AddBottomSpace(float space = 7.5f)
        {
            this.SetMarginBottom(space);
            return this;
        }
        #endregion

        #region Flex Component Icon
        public FlexComponentHeader AddIcon(string imagePath, MAddressType type)
        {
            this.AddElements(_flexIcon.SetIcon(imagePath, type)).AddVSpace(3);
            return this;
        }
        public FlexComponentHeader AddIcon(Texture2D image)
        {
            this.AddElements(_flexIcon.SetIcon(image)).AddVSpace(3);
            return this;
        }
        public FlexComponentHeader SetIconColor(Color color)
        {
            _flexIcon.SetGlobalColor(color);
            return this;
        }
        #endregion

        #region Flex Component Title
        public FlexComponentHeader AddTitle(string title)
        {
            this.AddElements(_flexTitle.SetTitle(title));
            return this;
        }
        public FlexComponentHeader AddDocumentation(string href)
        {
            _flexTitle.AddDocumentation(href);
            return this;
        }
        #endregion
    }
}
#endif