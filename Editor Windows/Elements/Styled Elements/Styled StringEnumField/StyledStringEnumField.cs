#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using YNL.Extensions.Methods;
using YNL.Editors.Windows.Utilities;

namespace YNL.Editors.UIElements.Styled
{
    public class StyledStringEnumField : VisualElement
    {
        private const string _styleSheet = "Style Sheets/Elements/Styled/StyledStringEnumField";

        public Label Label;
        public PopupField<string> Popup;

        public Action<string> SelectionChanged;

        public StyledStringEnumField(string label, List<string> options, Action<string> selectionChanged = null, string defaultOption = "")
        {
            SelectionChanged = selectionChanged;
            options.Insert(0, "_");

            this.AddStyle(_styleSheet).AddClass("Main");

            Label = new Label(label).AddClass("Label");

            Popup = new PopupField<string>(options, 0).AddClass("Popup");
            if (!options.Contains(defaultOption) || defaultOption.IsNullOrEmpty()) Popup.value = options[0];
            else Popup.value = options[options.IndexOf(defaultOption)];
            Popup.RegisterValueChangedCallback(OnValueChanged);

            Add(Label);
            Add(Popup);
        }

        private void OnValueChanged(ChangeEvent<string> evt)
        {
            SelectionChanged?.Invoke(Popup.value);
        }
    }
}
#endif