#if UNITY_EDITOR
using System;
using UnityEngine.UIElements;
using YNL.EditorsObsoleted.Extensions;
using YNL.Extensions.Addons;
using YNL.Extensions.Methods;

namespace YNL.EditorsObsoleted.UIElements.Styled
{
    public class StyledSlider : VisualElement
    {
        private const string _styleSheet = "Style Sheets/Elements/Styled/StyledSlider";

        public Slider Slider;

        private MRange _range;

        public Action<float> OnValueChanged;

        public StyledSlider(MRange range) : base()
        {
            _range = range;

            this.AddStyle(_styleSheet, ESheet.Font).AddClass("Main");

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
            OnValueChanged?.Invoke(Slider.value.Remap(new(0, 10), _range));
        }
    }
}
#endif