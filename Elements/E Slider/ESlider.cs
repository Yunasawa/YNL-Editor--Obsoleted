#if UNITY_EDITOR
using System;
using UnityEngine.UIElements;
using YNL.Editor.Extension;
using YNL.Editor.Utility;

namespace YNL.Editor.UIElement
{
    public class ESlider : VisualElement
    {
        private const string _styleSheet = "Style Sheets/Elements/ESlider";

        public Slider Slider;

        private EMinMax _range;

        public Action<float> OnValueChanged;

        public ESlider(EMinMax range) : base()
        {
            _range = range;

            this.AddStyle(_styleSheet, EAddress.USSFont).AddClass("Main");

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