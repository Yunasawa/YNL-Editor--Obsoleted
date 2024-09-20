#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Toolbars;
using YNL.Editors.Extensions;

namespace YNL.Editors.Windows.AnimationObjectRenamer
{
    [InitializeOnLoad]
    public static class Toolbar
    {
        private const string _styleSheet = "Style Sheets/Windows/General Toolbox/Animation - Object Renamer/Toolbar";

        public static EditorToolbarButton Button;

        static Toolbar()
        {
            EditorApplication.update += OnEditorUpdate;

            Variable.OnModeChanged -= UpdateButton;
            Variable.OnModeChanged += UpdateButton;
        }

        private static void OnEditorUpdate()
        {
            EditorApplication.update -= OnEditorUpdate;

            var rightToolbar = EEditor.Editor.GetToolbarZoneRight().AddStyle(_styleSheet);

            Button = new EditorToolbarButton(EnableAOR);
            Button.AddClass("Button").SetBackgroundImageTintColor(Variable.IsAutomaticOn ? "#52ffa8" : "#ff5252"); ;

            rightToolbar.Add(Button);
            
            void EnableAOR()
            {
                Variable.IsAutomaticOn = !Variable.IsAutomaticOn;
                Variable.OnModeChanged?.Invoke();

                UpdateButton();
            }
        }

        private static void UpdateButton()
        {
            Button.SetTooltip(Variable.IsAutomaticOn ? "Animation Object Renamer: On" : "Animation Object Renamer: Off");
            Button.SetBackgroundImageTintColor(Variable.IsAutomaticOn ? "#52ffa8" : "#ff5252");
        }
    }
}
#endif