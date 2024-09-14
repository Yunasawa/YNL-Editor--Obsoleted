#if UNITY_EDITOR && YNL_UTILITIES
using UnityEditor;
using UnityEngine;
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
        public Button Path;
        public GameObject BindedObject;
        public Button Count;
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

            string oldName = $"<color=#a6fff9>{log.Name.Split("|")[0]}</color>";
            string newName = $"<color=#a6fff9>{log.Name.Split("|")[1]}</color>";

            string oldPath = log.Path.Split("|")[0].Replace(log.Name.Split("|")[0], oldName);
            string newPath = log.Path.Split("|")[1].Replace(log.Name.Split("|")[1], newName);

            string pathText = $"{oldPath.HighlightDifferences(newPath, true, header, footer)} ▶ {newName.HighlightDifferences(oldName, true, header, footer)}";

            BindedObject = log.BindedObject;
            Path = new Button().AddClass("Path").AddClass("OldPath").SetText($"<color=#ffffff>{pathText}</color>"); //f8ff9c
            Path.clicked += () => EditorGUIUtility.PingObject(BindedObject);

            string state = log.IsSucceeded ? "Succeeded" : "Failed";

            Count = new Button().AddClass("Path").AddClass("NewPath")
                .SetText($"{header}{state}{footer}: {log.AnimatorAmount} <color=#d6ffb3>Animator(s)</color> | {log.ClipAmount} <color=#d6ffb3>Clip(s)</color>");
            PathContainer = new VisualElement().AddClass("PathContainer").AddElements(Path, Count);

            this.AddElements(State, Time, PathContainer);
        }
    }
}
#endif