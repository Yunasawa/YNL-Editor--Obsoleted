using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using YNL.Editors.Windows.Utilities;
using YNL.Extensions.Methods;

namespace YNL.Editors.UIElements.Plained
{
    public class EFloatField : VisualElement
    {
        private const string _styleSheet = "Style Sheets/Elements/Plained/EFloatField";

        public FloatField Field;
        private VisualElement LabelField;
        private VisualElement InputField;

        public EFloatField(SerializedProperty serializedObject) : base()
        {
            this.AddStyle(_styleSheet, EStyleSheet.Font).AddClass("Main");

            Field = new FloatField(serializedObject.name.AddSpaces()).AddClass("Field", "unity-base-field__aligned");
            InputField = Field.Q("unity-text-input").AddClass("Input");
            LabelField = Field.Q(classes: "unity-label").AddClass("Label");

            this.AddElements(Field);

            Field.BindProperty(serializedObject);

            this.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
            this.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        }

        private void OnMouseEnter(MouseEnterEvent e)
        {
            InputField.EnableClass("Input_Enter");
        }

        private void OnMouseLeave(MouseLeaveEvent e)
        {
            InputField.DisableClass("Input_Enter");
        }
    }
}
