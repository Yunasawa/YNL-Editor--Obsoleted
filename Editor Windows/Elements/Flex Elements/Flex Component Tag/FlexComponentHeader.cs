using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editors.Windows.Utilities;
using YNL.Extensions.Addons;
using YNL.Extensions.Methods;

namespace YNL.Editors.UIElements.Flexs
{
    public class FlexComponentHeader : VisualElement
    {
        private const string _styleSheet = "Style Sheets/Elements/Specials/Flex Component Tag/FlexComponentHeader";

        private Color _globalColor = "#AFAFAF".ToColor();

        private FlexComponentTitle _flexTitle;
        private FlexComponentIcon _flexIcon;

        public FlexComponentHeader()
        {
            this.AddStyle(_styleSheet, EStyleSheet.Font).AddClass("Main");
        }

        public FlexComponentHeader SetGlobalColor(string color) => SetGlobalColor(color.ToColor());
        public FlexComponentHeader SetGlobalColor(Color color)
        {
            _globalColor = color;

            _flexTitle = new FlexComponentTitle().SetGlobalColor(_globalColor);
            _flexIcon = new FlexComponentIcon().SetGlobalColor(_globalColor);

            return this;
        }

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