#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EIcon : VisualElement
{
    private const string USS_StyleSheet = EAddress.FolderPath + "Scripts/Elements/E Icon/EIcon1.uss";

    private static readonly string USS_Background = "Background";
    private static readonly string USS_Image = "Image";

    public VisualElement Background;
    public Image Image;

    public EIcon() : base()
    {
        styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(USS_StyleSheet));

        Background = new Image();
        Background.AddToClassList(USS_Background);

        Image = new();
        Image.style.backgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>(EAddress.FolderPath + "Textures/Running Guy.png");
        Image.AddToClassList(USS_Image);

        Background.Add(Image);

        this.Add(Background);
    }
}
#endif
