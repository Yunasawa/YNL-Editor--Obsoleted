#if UNITY_EDITOR && YNL_UTILITIES
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editors.Extensions;
using YNL.Extensions.Methods;

namespace YNL.Editors.UIElements.Flex
{
    public class FlexComponentTitle : VisualElement
    {
        private const string _styleSheet = "Style Sheets/Elements/Specials/Flex Component Tag/FlexComponentTitle";

        private Color _globalColor = "#AFAFAF".ToColor();

        private Label _title;
        private Image _container;
        private Button _documentation;

        public FlexComponentTitle()
        {
            this.AddStyle(_styleSheet, ESheet.Font).AddClass("Main");

            _title = new Label().AddClass("Title");
            _container = new Image().AddClass("Container");

            this.AddElements(_title, _container);

            this.RegisterCallback<MouseEnterEvent>((evt) =>
            {
                _title.EnableClass("Title_Enter");
            });
            this.RegisterCallback<MouseLeaveEvent>((evt) =>
            {
                _title.DisableClass("Title_Enter");
            });
        }

        public FlexComponentTitle SetGlobalColor(Color color)
        {
            _globalColor = color;
            _title.SetColor(color);
            return this;
        }

        public FlexComponentTitle SetTitle(string title)
        {
            _title.SetText(title);
            return this;
        }

        public FlexComponentTitle AddDocumentation(string href)
        {
            _documentation = new Button().AddClass("Documentation");
            _container.AddElements(_documentation);

            _documentation.SetTooltip("Documentation");
            _documentation.SetHyperlink(href);
            _documentation.RegisterCallback<MouseEnterEvent>((evt) => _documentation.SetBackgroundImageTintColor(_globalColor));
            _documentation.RegisterCallback<MouseLeaveEvent>((evt) => _documentation.SetBackgroundImageTintColor("#AFAFAF".ToColor()));
            
            
            return this;
        }
    }
}
#endif