using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine.UIElements;
using YNL.Editors.Windows.Utilities;
using System;
using YNL.Extensions.Methods;
using UnityEngine.UI;

namespace YNL.Editors.UIElements.Plained
{
    public class PlainedEnumField : VisualElement
    {
        private const string _styleSheet = "Style Sheets/Elements/Plained/PlainedEnumField";

        public EnumField Field;
        private VisualElement LabelField;
        private VisualElement EnumField;

        public PlainedEnumField(SerializedProperty serializedObject, string name) : base()
        {
            this.AddStyle(_styleSheet, EStyleSheet.Font).AddClass("Main");

            Type type = MType.GetType(name);

            Field = new EnumField(serializedObject.name.AddSpaces(), (Enum)Activator.CreateInstance(type)).AddClass("Field", "unity-base-field__aligned");
            EnumField = Field.Q(classes: "unity-enum-field__input").AddClass("Input");
            LabelField = Field.Q(classes: "unity-label").AddClass("Label");

            this.AddElements(Field);

            Field.BindProperty(serializedObject);

            this.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
            this.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        }

        private void OnMouseEnter(MouseEnterEvent e)
        {
            EnumField.EnableClass("Input_Enter");
        }

        private void OnMouseLeave(MouseLeaveEvent e)
        {
            EnumField.DisableClass("Input_Enter");
        }
    }
}
