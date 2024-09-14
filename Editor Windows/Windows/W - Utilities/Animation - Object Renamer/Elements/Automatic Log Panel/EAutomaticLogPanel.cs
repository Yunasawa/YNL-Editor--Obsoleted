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
        public Image ClearLog;
        public Button ClearLogButton;

        public ScrollView LogScroll;

        public EAutomaticLogPanel() : base()
        {
            this.AddStyle(USS_StyleSheet, EStyleSheet.Font).SetName("Root");

            TagIcon = new Image().SetName("TagIcon");
            Line = new StyledLine(ELineMode.Vertical).AddClass("Line");
            Title = new Label("Automatic log panel").SetName("Label");

            ClearLogButton = new Button().AddClass("ClearLogButton").SetText("Clear Logs");
            ClearLogButton.clicked += Visual.ClearLogPanel;
            ClearLog = new Image().AddClass("ClearLog").AddElements(ClearLogButton);

            TitleBackground = new Image().SetName("TitleBackground").AddElements(TagIcon, Line, Title, ClearLog);

            LogScroll = new ScrollView().SetName("LogScroll");

            this.AddElements(TitleBackground, LogScroll);
        }

        public void AddLogItem(EAutomaticLogLine line)
        {
            LogScroll.InsertElements(0, line);
        }

        public void ClearLogs() => LogScroll.Clear();

        public void ClearAllClipItem() => LogScroll.RemoveAllElements();
    }
}
#endif