#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using YNL.Editor.Extensions;
using YNL.Editor.Utilities;

namespace YNL.Editor.UIElement
{
    public class EWindowTagPanel : EInteractableImage
    {
        private const string _uss_StyleSheet = "Style Sheets/Elements/EWindowTagPanel";

        private static readonly string _uss_panelBackground = "panel-background";
        private static readonly string _uss_titleBackground = "title-background";
        private static readonly string _uss_icon = "icon";
        private static readonly string _uss_iconHover = "icon__hover";
        private static readonly string _uss_title = "title";
        private static readonly string _uss_titleHover = "title__hover";
        private static readonly string _uss_subtitle = "subtitle";
        private static readonly string _uss_subtitleHover = "subtitle__hover";

        public Image TitleBackground = new Image();
        public Image Icon = new Image();
        public Label Title = new Label();
        public Label Subtitle = new Label();
        public ScrollView Scroll = new ScrollView();

        public Image TutorialBackground = new Image();
        public Button Tutorial = new Button();
        public Image TutorialIcon = new Image();
        public Label TutorialLabel = new Label("Instruction");

        public EWindowTag[] Tags = new EWindowTag[0];

        private int _selectedTag = 0;
        private float _maxWidth = 0;

        public EWindowTagPanel(Texture2D icon, string title, string subtitle, float maxWidth, EWindowTag[] tags) : base()
        {
            this.AddStyle(_uss_StyleSheet, EAddress.USSFont);

            Icon.SetBackgroundImage(icon).AddClass(_uss_icon);
            Title.SetText(title).AddClass(_uss_title);
            Subtitle.SetText(subtitle).AddClass(_uss_subtitle);

            TitleBackground.AddClass(_uss_titleBackground).AddElements(Icon, Title, Subtitle);

            TutorialIcon.AddClass("TutorialIcon");
            TutorialLabel.AddClass("TutorialLabel");
            Tutorial.AddClass("Tutorial").AddElements(TutorialIcon, TutorialLabel);
            TutorialBackground.AddClass("TutorialBackground").AddElements(Tutorial);

            this.AddClass(_uss_panelBackground).AddElements(TitleBackground, TutorialBackground).AddSpace(0, 100);

            Tags = tags;

            Scroll.AddClass("Scroll");

            foreach (var tag in Tags)
            {
                Scroll.AddElements(tag).AddSpace(0, 45);
                tag.OnClick += () => SelectTag(tag, false);
            }

            this.AddElements(Scroll);

            SelectTag(Tags[_selectedTag], true);

            _maxWidth = maxWidth;
        }

        public override void PointerEnter()
        {
            this.SetWidth(_maxWidth);

            Icon.EnableClass(true, _uss_iconHover);
            Title.EnableClass(true, _uss_titleHover);
            Subtitle.EnableClass(true, _uss_subtitleHover);

            Tutorial.EnableClass(true, "Tutorial".Custom("Enter"));
            TutorialIcon.EnableClass(true, "TutorialIcon".Custom("Enter"));
            TutorialLabel.EnableClass(true, "TutorialLabel".Custom("Enter"));

            foreach (var tag in Tags) tag.OnExpand();
        }
        public override void PointerExit()
        {
            this.SetWidth(50);

            Icon.EnableClass(false, _uss_iconHover);
            Title.EnableClass(false, _uss_titleHover);
            Subtitle.EnableClass(false, _uss_subtitleHover);

            Tutorial.EnableClass(false, "Tutorial".Custom("Enter"));
            TutorialIcon.EnableClass(false, "TutorialIcon".Custom("Enter"));
            TutorialLabel.EnableClass(false, "TutorialLabel".Custom("Enter"));

            foreach (var tag in Tags) tag.OnCollape();
        }

        public void SelectTag(EWindowTag tag, bool forceSelect)
        {
            if (!forceSelect && Tags[_selectedTag] == tag) return;

            _selectedTag = Tags.IndexOf(tag);

            foreach (var item in Tags) item.OnDeselect();
            tag.OnSelect();
        }
        public void ForceSelectTag(int index)
        {
            if (_selectedTag == index) return;

            _selectedTag = index;
            EWindowTag tag = Tags[_selectedTag];
            SelectTag(tag, true);
        }
    }
}
#endif
