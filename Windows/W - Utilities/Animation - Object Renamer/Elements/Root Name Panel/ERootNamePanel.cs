#if UNITY_EDITOR
using UnityEngine.UIElements;
using YNL.Editor.UIElement;
using YNL.Editor.Utility;

namespace YNL.Editor.Window.Animation.ObjectRenamer
{
    public class ERootNamePanel : Button
    {
        private const string USS_StyleSheet = "Style Sheets/Windows/W - Utilities/Animation - Object Renamer/ERootNamePanel";

        public Image TitleBackground;
        public Image TagIcon;
        public ELine Line;
        public Label Title;
        public ScrollView ClipPanel;
        public Label Board;

        public ERootNamePanel() : base()
        {
            this.AddStyle(USS_StyleSheet, EAddress.USSFont).SetName("Root");

            TagIcon = new Image().SetName("TagIcon");
            Line = new ELine(ELineMode.Vertical).AddClass("Line");
            Title = new Label("Referenced objects full path").SetName("Label");
            TitleBackground = new Image().SetName("TitleBackground").AddElements(TagIcon, Line, Title);

            ClipPanel = new ScrollView().SetName("ClipPanel");

            AddBoard("No animation clip found!");

            this.AddElements(TitleBackground, ClipPanel);
        }

        public void AddClipItem(EClipNameField field)
        {
            ClipPanel.AddElements(field);
        }

        public void ClearAllClipItem() => ClipPanel.RemoveAllElements();
        public void AddBoard(string text)
        {
            Board = new Label().SetText(text).AddClass("Board");
            ClipPanel.AddElements(Board);
        }
    }
}
#endif