#if UNITY_EDITOR
using UnityEngine.UIElements;
using YNL.Editors.UIElements.Styled;
using YNL.Editors.Windows.Utilities;

namespace YNL.Editors.Windows.AnimationObjectRenamer
{
    public class EAutomaticLogPanel : Button
    {
        private const string USS_StyleSheet = "Style Sheets/Windows/W - Utilities/Animation - Object Renamer/EAutomaticLogPanel";

        public Image TitleBackground;
        public Image TagIcon;
        public StyledLine Line;
        public Label Title;

        public ScrollView LogScroll;

        public EAutomaticLogPanel() : base()
        {
            this.AddStyle(USS_StyleSheet, EStyleSheet.Font).SetName("Root");

            TagIcon = new Image().SetName("TagIcon");
            Line = new StyledLine(ELineMode.Vertical).AddClass("Line");
            Title = new Label("Automatic log panel").SetName("Label");
            TitleBackground = new Image().SetName("TitleBackground").AddElements(TagIcon, Line, Title);

            LogScroll = new ScrollView().SetName("LogScroll");

            this.AddElements(TitleBackground, LogScroll);
        }

        public void AddClipItem(EAutomaticLogLine line)
        {
            LogScroll.InsertElements(0, line);
        }

        public void ClearAllClipItem() => LogScroll.RemoveAllElements();
    }
}
#endif