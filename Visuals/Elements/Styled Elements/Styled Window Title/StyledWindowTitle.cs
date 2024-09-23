#if UNITY_EDITOR
using UnityEngine.UIElements;
using UnityEngine;
using YNL.Extensions.Methods;
using YNL.EditorsObsoleted.Extensions;

namespace YNL.EditorsObsoleted.UIElements.Styled
{
    public class StyledWindowTitle : VisualElement
    {
        private const string USS_StyleSheet = "Style Sheets/Elements/Styled/StyledWindowTitle";

        private const string _uss_panel = "panel";
        private const string _uss_icon = "icon";
        private const string _uss_icon_hover = "icon__hover";
        private const string _uss_title = "title";
        private const string _uss_subtitle = "subtitle";

        public Button Panel;
        public Image Icon;
        public Label Title;
        public Label Subtitle;

        public StyledWindowTitle(Texture2D icon, string title, string subtitle) : base()
        {
            this.AddStyle(USS_StyleSheet, ESheet.Font);

            Icon = new Image();
            Icon.style.backgroundImage = icon;
            Icon.AddClass(_uss_icon);

            Title = new Label(title);
            Title.AddClass(_uss_title);

            Subtitle = new Label(subtitle);
            Subtitle.AddClass(_uss_subtitle);

            Panel = new Button();
            Panel.AddClass(_uss_panel);
            Panel.AddElements(Icon, Title, Subtitle);

            this.AddElements(Panel);

            RegisterCallback<PointerEnterEvent>(evt => OnPointerEnter(evt));
            RegisterCallback<PointerLeaveEvent>(evt => OnPointerLeave(evt));
        }

        private static void OnPointerEnter(PointerEnterEvent evt)
        {
            var panel = evt.currentTarget as StyledWindowTitle;

            panel.Icon.EnableClass(true, _uss_icon_hover);

            evt.StopPropagation();
        }
        private static void OnPointerLeave(PointerLeaveEvent evt)
        {
            var panel = evt.currentTarget as StyledWindowTitle;

            panel.Icon.EnableClass(false, _uss_icon_hover);

            evt.StopPropagation();
        }
    }
}
#endif