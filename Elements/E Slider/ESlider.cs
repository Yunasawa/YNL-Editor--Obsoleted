#if UNITY_EDITOR
using System;
using UnityEngine.UIElements;
using YNL.Editor.Extensions;
using YNL.Editor.Utilities;

public class ESlider : VisualElement
{
    private const string _styleSheet = EAddress.FolderPath + "Elements/E Slider/ESlider.uss";

    public Slider Slider;

    private MinMax _range;

    public Action<float> OnValueChanged;

    public ESlider(MinMax range) : base()
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
        OnValueChanged?.Invoke(Slider.value.Map(new(0, 10), _range));
    }
}
#endif