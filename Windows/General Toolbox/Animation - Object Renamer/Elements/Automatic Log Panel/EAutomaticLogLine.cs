#if UNITY_EDITOR && YNL_UTILITIES
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editors.Extensions;
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

        public EAutomaticLogLine(AORSettings.AutomaticLog log) : base()
        {
            this.AddStyle(USS_StyleSheet, ESheet.Font).AddClass("Root");

            State = new Image().AddClass("State").SetBackgroundColor(log.IsSucceeded ? "#3dffa8" : "#ff4a4a")
                .SetBackgroundImage(log.Event == AORSettings.Event.Rename ? "Textures/Icons/Rename" : "Textures/Icons/Move")
                .SetTooltip("State of previous automatic action");

            Time = new Button().AddClass("Time").SetText(log.CurrentTime);

            string header = log.IsSucceeded ? "<color=#3dffa8>" : "<color=#ff4a4a>";
            string footer = "</color>";

            string logOldName = log.Name.Split("|")[0];
            string logNewName = log.Name.Split("|")[1];

            string oldName = $"<color=#a6fff9>{logOldName.FillSpace(logNewName.Length)}</color>";
            string newName = $"<color=#a6fff9>{logNewName.FillSpace(logOldName.Length)}</color>";

            string oldPath = $"{log.BindedObject.GetPath(false)}/{oldName}";
            string newPath = $"{log.BindedObject.GetPath(false)}/{newName}";

            if (log.Event == AORSettings.Event.Destroy)
            {
                header = "<color=#ffdb4a>";
                State.SetBackgroundImage("Textures/Icons/Remove").SetBackgroundColor("#ffdb4a");
            }

            string pathText = $"{oldPath.HighlightDifferences(newPath, true, header, footer)} ▶ {newName.HighlightDifferences(oldName, true, header, footer)}";

            BindedObject = log.BindedObject;
            Path = new Button().AddClass("Path").AddClass("OldPath").SetText(pathText); //f8ff9c
            Path.clicked += () =>
            {
                if (BindedObject.IsNull()) return;
                EditorGUIUtility.PingObject(BindedObject);
            };

            Count = new Button().AddClass("Path").AddClass("NewPath")
                .SetText($"{header}{log.Event}{footer}: {log.AnimatorAmount} <color=#d6ffb3>Animator(s)</color> | {log.ClipAmount} <color=#d6ffb3>Clip(s)</color>");
            PathContainer = new VisualElement().AddClass("PathContainer").AddElements(Path, Count);

            this.AddElements(State, Time, PathContainer);
        }
    }
}
#endif