#if UNITY_EDITOR && YNL_UTILITIES
using UnityEngine.UIElements;
using YNL.Editors.Windows.Utilities;
using YNL.Extensions.Methods;

namespace YNL.Editors.Windows.AnimationObjectRenamer
{
    public class EAutomaticLogLine : Button
    {
        private const string USS_StyleSheet = "Style Sheets/Windows/W - Utilities/Animation - Object Renamer/EAutomaticLogLine";

        public Image State;
        public Button Time;
        public Button OldPath;
        public Button NewPath;
        public VisualElement PathContainer;

        public EAutomaticLogLine(AnimationObjectRenamerSettings.AutomaticLog log) : base()
        {
            this.AddStyle(USS_StyleSheet, EStyleSheet.Font).AddClass("Root");

            State = new Image().AddClass("State").SetBackgroundColor(log.IsSucceeded ? "#3da878" : "#a83d3d")
                .SetBackgroundImage(log.IsSucceeded ? "Textures/Icons/V" : "Textures/Icons/X");

            Time = new Button().AddClass("Time").SetText(log.CurrentTime);

            string header = "<color=#ff6e6e>";
            string footer = "</color>";

            OldPath = new Button().AddClass("Path").AddClass("OldPath")
                .SetText($"{log.OldPath.HighlightDifferences(log.NewPath, true, header, footer)}");
            NewPath = new Button().AddClass("Path").AddClass("NewPath")
                .SetText($"<color=#f8ff9c>▶ {log.NewPath.HighlightDifferences(log.OldPath, true, header, footer)}");
            PathContainer = new VisualElement().AddClass("PathContainer").AddElements(OldPath, NewPath);

            this.AddElements(State, Time, PathContainer);
        }
    }
}
#endif