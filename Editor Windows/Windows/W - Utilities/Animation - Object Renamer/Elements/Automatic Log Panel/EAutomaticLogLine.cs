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
                .SetBackgroundImage(log.IsSucceeded ? "Textures/Icons/V" : "Textures/Icons/X")
                .SetTooltip("State of previous automatic action");

            Time = new Button().AddClass("Time").SetText(log.CurrentTime);

            string header = log.IsSucceeded ? "<color=#3dffb5>" : "<color=#fa7373>";
            string footer = "</color>";

            MDebug.Notify($"{log.OldName} - {log.NewName}");

            string oldName = $"<color=#a6fff9>{log.OldName}</color>";
            string newName = $"<color=#a6fff9>{log.NewName}</color>";

            string oldPath = log.OldPath.Replace(log.OldName, oldName);
            string newPath = log.NewPath.Replace(log.NewName, newName);

            string pathText = $"{oldPath.HighlightDifferences(newPath, true, header, footer)} ▶ {newName.HighlightDifferences(oldName, true, header, footer)}";

            OldPath = new Button().AddClass("Path").AddClass("OldPath").SetText($"<color=#f8ff9c>{pathText}</color>");
            NewPath = new Button().AddClass("Path").AddClass("NewPath")
                .SetText($"Changed: 1 <color=#17ffa6>Animator(s)</color> | 5 <color=#63ffff>Animation Clip(s)</color>");
            PathContainer = new VisualElement().AddClass("PathContainer").AddElements(OldPath, NewPath);

            this.AddElements(State, Time, PathContainer);
        }
    }
}
#endif