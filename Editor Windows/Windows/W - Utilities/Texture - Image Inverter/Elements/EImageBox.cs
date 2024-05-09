#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editors.Windows.Utilities;

namespace YNL.Editors.Windows.Texture.ImageInverter
{
    public class EImageBox : VisualElement
    {
        private const string _styleSheet = "Style Sheets/Windows/W - Utilities/Texture - Image Inverter/EImageBox";

        public Texture2D AssignedImage;

        public Button Background;
        public Button Delete;
        public Button Name;

        public Image ImageBackground;
        public Image Arrow;
        public Image OriginalImage;
        public Image InvertedImage;

        public EImageBox(Texture2D image) : base()
        {
            if (image.EIsNull())
            {
                EDebug.ENotify("Image not found, something is wrong!");
                return;
            }

            AssignedImage = image;

            this.AddStyle(_styleSheet, EAddress.USSFont).AddClass("Main");

            OriginalImage = new Image().SetBackgroundImage(image).AddClass("Image", "OriginalImage");
            InvertedImage = new Image().SetBackgroundImage(WTextureImageInverter_Handler.InvertImage(image)).AddClass("Image", "InvertedImage");

            Arrow = new Image().AddClass("Arrow");

            ImageBackground = new Image().AddClass("ImageBackground");
            ImageBackground.AddElements(OriginalImage, Arrow, InvertedImage);

            Delete = new Button().AddClass("Delete");
            Delete.clicked += () =>
            {
                this.RemoveFromHierarchy();
                WTextureImageInverter_Main.OnRemoveImage?.Invoke(AssignedImage);
            };

            Name = new Button().AddClass("Name").SetText(image.name);
            Name.clicked += () => EditorGUIUtility.PingObject(image);

            Background = new Button().AddClass("Background").AddElements(Name, Delete, ImageBackground);

            this.AddElements(Background);
        }

        public void UpdateInvertedImage(Texture2D originalImage, Texture2D invertedImage)
        {
            OriginalImage.SetBackgroundImage(originalImage);
            InvertedImage.SetBackgroundImage(InvertImage(originalImage));
        }

        private Texture2D InvertImage(Texture2D originalTexture)
        {
            if (originalTexture == null)
            {
                EDebug.EError("<b>Original texture</b> is null. Cannot invert.");
                return null;
            }

            // Ensure the texture is readable and writable
            if (!originalTexture.isReadable)
            {
                EDebug.EWarning($"Texture is not readable. Adjusting import settings to readable");
                string assetPath = AssetDatabase.GetAssetPath(originalTexture);
                TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(assetPath);
                textureImporter.isReadable = true;
                AssetDatabase.ImportAsset(assetPath);
            }

            int width = originalTexture.width;
            int height = originalTexture.height;

            Color[] pixels = originalTexture.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
            {
                Color originalColor = pixels[i];

                // Preserve transparency (alpha channel)
                Color invertedColor = new Color(1f - originalColor.r, 1f - originalColor.g, 1f - originalColor.b, originalColor.a);

                pixels[i] = invertedColor;
            }

            Texture2D invertedTexture = new Texture2D(width, height);
            invertedTexture.SetPixels(pixels);
            invertedTexture.Apply();

            return invertedTexture;
        }
    }
}
#endif