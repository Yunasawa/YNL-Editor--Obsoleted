#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ELabel : VisualElement
{
    private const string USS_StyleSheet = "Style Sheets/Elements/ELabel1";

    private static readonly string USS_Container = "Container";
    private static readonly string USS_Background = "Background";
    private static readonly string USS_Label = "Label";
    private static readonly string USS_BackgroundIcon = "BackgroundIcon";

    public VisualElement _container;
    public EIcon Icon;
    private VisualElement _background;
    public Label Label;
    public Image BackgroundIcon;

    /// <summary>
    /// Use UpdateGUI() in OnGUI() to update Elabel's elements.
    /// </summary>
    public ELabel() : base()
    {
        styleSheets.Add(Resources.Load<StyleSheet>(USS_StyleSheet));

        _container = new();
        _container.AddToClassList(USS_Container);

        Icon = new EIcon();
        _container.Add(Icon);

        _background = new Button();
        _background.AddToClassList(USS_Background);

        BackgroundIcon = new();
        BackgroundIcon.AddToClassList(USS_BackgroundIcon);
        BackgroundIcon.style.backgroundImage = Resources.Load<Texture2D>("Textures/Running Guy");
        _background.Add(BackgroundIcon);

        Label = new("Animation");
        Label.AddToClassList(USS_Label);
        _background.Add(Label);

        _container.Add(_background);
        
        this.Add(_container);
    }

    public void UpdateGUI()
    {
        _background.style.width = _container.resolvedStyle.width - 60;
    }
}
#endif