#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UIElements;

namespace YNL.Editors.UIElements
{
    public class EIcon : VisualElement
    {
        private const string USS_StyleSheet = "Style Sheets/Elements/EIcon1";

        private static readonly string USS_Background = "Background";
        private static readonly string USS_Image = "Image";

        public VisualElement Background;
        public Image Image;

        public EIcon() : base()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(USS_StyleSheet));

            Background = new Image();
            Background.AddToClassList(USS_Background);

            Image = new();
            Image.style.backgroundImage = Resources.Load<Texture2D>("Textures/Icons/Target");
            Image.AddToClassList(USS_Image);

            Background.Add(Image);

            this.Add(Background);
        }
    }
}
#endif
