using System;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editors.Windows.Utilities;
using YNL.Extensions.Methods;

namespace YNL.Editors.UIElements.Plained
{
    public class PlainedIntField : VisualElement
    {
        private const string _styleSheet = "Style Sheets/Elements/Plained/PlainedIntField";

        public IntegerField Field;
        private VisualElement LabelField;
        private VisualElement InputField;

        public PlainedIntField(SerializedProperty serializedObject) : base()
        {
            this.AddStyle(_styleSheet, EStyleSheet.Font).AddClass("Main");

            Field = new IntegerField(serializedObject.name.AddSpaces()).AddClass("Field", "unity-base-field__aligned");
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
