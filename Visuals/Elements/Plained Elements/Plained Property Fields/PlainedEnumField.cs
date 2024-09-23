#if UNITY_EDITOR && YNL_UTILITIES
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine.UIElements;
using YNL.Extensions.Methods;
using System;
using YNL.EditorsObsoleted.Extensions;
using UnityEngine;

namespace YNL.EditorsObsoleted.UIElements.Plained
{
    public class PlainedEnumField : VisualElement
    {
        private const string _styleSheet = "Style Sheets/Elements/Plained/PlainedEnumField";

        private EnumField _field;
        private VisualElement _labelField;
        private VisualElement _enumField;

        public PlainedEnumField(SerializedProperty serializedObject, string name) : base()
        {
            this.AddStyle(_styleSheet, ESheet.Font).AddClass("Main");

            Type type = MType.GetTypeIgnoreAssembly(name);

            _field = new EnumField(serializedObject.name.AddSpaces(), (Enum)Activator.CreateInstance(type)).AddClass("Field", "unity-base-field__aligned");
            _enumField = _field.Q(classes: "unity-enum-field__input").AddClass("Input");
            _labelField = _field.Q(classes: "unity-label").AddClass("Label");

            this.AddElements(_field);

            _field.BindProperty(serializedObject);

            this.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
            this.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        }

        private void OnMouseEnter(MouseEnterEvent e)
        {
            _enumField.EnableClass("Input_Enter");
        }

        private void OnMouseLeave(MouseLeaveEvent e)
        {
            _enumField.DisableClass("Input_Enter");
        }

        #region Style
        public PlainedEnumField SetAsBoxedField(float maxWidth = -1, bool isPercent = false)
        {
            this.SetPadding(5).SetPaddingTop(2.5f).SetBackgroundColor("#303030")
                .SetBorderRadius(5).SetHeight(47.5f).SetMargin(0).SetMarginBottom(5);

            if (maxWidth == -1) this.SetWidth(StyleKeyword.Auto);
            else this.SetWidth(maxWidth, isPercent);

            _labelField.SetTextAlign(TextAnchor.UpperLeft);

            _field.SetFlexDirection(FlexDirection.Column);

            return this;
        }
        #endregion
    }
}
#endif