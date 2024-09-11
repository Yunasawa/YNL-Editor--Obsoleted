#if UNITY_EDITOR && YNL_UTILITIES
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine.UIElements;
using YNL.Extensions.Methods;
using YNL.Editors.Windows.Utilities;

namespace YNL.Editors.UIElements.Plained
{
    public class PlainedToggleField : VisualElement
    {
        private const string _styleSheet = "Style Sheets/Elements/Plained/PlainedToggleField";

        public Toggle Field;
        private VisualElement LabelField;
        private VisualElement Checkmark;

        public PlainedToggleField(SerializedProperty serializedObject) : base()
        {
            this.AddStyle(_styleSheet, EStyleSheet.Font).AddClass("Main");

            Field = new Toggle(serializedObject.name.AddSpaces()).AddClass("Field", "unity-base-field__aligned");
            LabelField = Field.Q(classes: "unity-label").AddClass("Label");
            Checkmark = Field.Q("unity-checkmark").AddClass("Checkmark");

            this.AddElements(Field);

            Field.BindProperty(serializedObject);

            this.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
            this.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);

            Field.RegisterValueChangedCallback(OnChangeValue);
        }

        private void OnMouseEnter(MouseEnterEvent e)
        {
            Checkmark.EnableClass("Checkmark_Enter");
        }

        private void OnMouseLeave(MouseLeaveEvent e)
        {
            Checkmark.DisableClass("Checkmark_Enter");
        }

        private void OnChangeValue(ChangeEvent<bool> evt)
        {
            if (evt.newValue) Checkmark.EnableClass("Checkmark_True");
            else Checkmark.DisableClass("Checkmark_True");
        }
    }
}
#endif