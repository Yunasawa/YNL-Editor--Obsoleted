#if UNITY_EDITOR
using System;
using UnityEngine.UIElements;
using YNL.Editors.Windows.Utilities;

namespace YNL.Editors.UIElements.Styled
{
    public class StyledSlider : VisualElement
    {
        private const string _styleSheet = "Style Sheets/Elements/Styled/StyledSlider";

        public Slider Slider;

        private EMinMax _range;

        public Action<float> OnValueChanged;

        public StyledSlider(EMinMax range) : base()
        {
            _range = range;

            this.AddStyle(_styleSheet, EStyleSheet.Font).AddClass("Main");

            Slider = new Slider().AddClass("Slider");
            Slider.Q("unity-dragger").AddClass("Dragger");
            Slider.Q("unity-dragger-border").RemoveFromHierarchy();
            Slider.Q("unity-tracker").AddClass("Tracker");

            Label label = new Label("Size").AddClass("Label");

            this.AddElements(Slider);

            Slider.RegisterValueChangedCallback(ValueChanged);
        }

        public void ValueChanged(ChangeEvent<float> evt)
        {
            OnValueChanged?.Invoke(Slider.value.ERemap(new(0, 10), _range));
        }
    }
}
#endif