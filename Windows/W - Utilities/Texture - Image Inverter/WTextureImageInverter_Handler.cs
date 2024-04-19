#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YNL.Editor.Extension;

namespace YNL.Editor.Window.Texture.ImageInverter
{
    public class WTextureImageInverter_Handler
    {
        #region ▶ Fields/Properties
        private WTextureImageInverter_Main _main;

        public List<Texture2D> Textures = new();
        #endregion

        public WTextureImageInverter_Handler(WTextureImageInverter_Main main)
        {
            _main = main;

            WTextureImageInverter_Main.OnAddImage += GetAllImages;
            WTextureImageInverter_Main.OnRemoveImage += RemoveImage;
        }

        public void GetAllImages(UnityEngine.Object[] assets)
        {
            foreach (var asset in assets) GetAllImage(asset);

            _main.Visual.GenerateImages(Textures.ToArray(), _main.Visual.ImageWidth);
        }
        public void GetAllImage(UnityEngine.Object item)
        {
            string path = AssetDatabase.GetAssetPath(item);

            if (item is DefaultAsset)
            {
                string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { path });
                foreach (var guid in guids)
                {
                    string getPath = AssetDatabase.GUIDToAssetPath(guid);
                    Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(getPath);
                    Textures.EAddDistinct(texture);
                }
            }
            else if (item is Texture2D)
            {
                Textures.EAddDistinct(item as Texture2D);
            }
        }

        public void InvertImage(Texture2D inputImage, bool replaceOldTexture, EImageBox box)
        {
            StartInvert(inputImage, replaceOldTexture, box);
        }

        public void StartInvert(Texture2D inputImage, bool replaceOldTexture, EImageBox box)
        {
            try
            {
                string outputPath = AssetDatabase.GetAssetPath(inputImage);
                Texture2D invertedTexture = InvertImage(inputImage);
                byte[] resizedBytes = invertedTexture.EncodeToPNG();
                if (replaceOldTexture) System.IO.File.WriteAllBytes(outputPath, resizedBytes);
                else System.IO.File.WriteAllBytes($"{outputPath.Replace(".", "(Copied).")}", resizedBytes);

                box.UpdateInvertedImage(invertedTexture, inputImage);
            }
            catch (System.Exception e)
            {
                EDebug.EError($"Error inverting image: {e.Message}");
            }
        }

        public static Texture2D InvertImage(Texture2D originalTexture)
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

        public void RemoveImage(Texture2D image)
        {
            Textures.Remove(image);
        }
    }
}
#endif