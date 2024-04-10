#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YNL.Editor.Extensions;

namespace YNL.Editor.Window.Texture.ImageResizer
{
    public class WTextureImageResizer_Handler
    {
        #region ▶ Fields/Properties
        private WTextureImageResizer_Main _main;

        public List<Texture2D> Textures = new();
        #endregion

        public WTextureImageResizer_Handler(WTextureImageResizer_Main main)
        {
            _main = main;

            WTextureImageResizer_Main.OnAddImage += GetAllImages;
            WTextureImageResizer_Main.OnRemoveImage += RemoveImage;
        }

        public void OnGUI()
        {

        }

        #region Texture Displayer
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
                    Textures.AddDistinct(texture);
                }
            }
            else if (item is Texture2D)
            {
                Textures.AddDistinct(item as Texture2D);
            }
        }
        public void GetAllImages(UnityEngine.Object[] assets)
        {
            foreach (var asset in assets) GetAllImage(asset);

            _main.Visual.GenerateImages(Textures.ToArray(), _main.Visual.ImageWidth);
        }

        public void RemoveImage(Texture2D image)
        {
            Textures.Remove(image);
        }
        #endregion
        #region Resizing Handler
        public void ResizeImage(Texture2D inputImage, Vector2 newSize, bool replaceOldTexture)
        {
            ResizeImage(inputImage, (int)newSize.x, (int)newSize.y, replaceOldTexture);

            AssetDatabase.Refresh();
        }

        private void ResizeImage(Texture2D inputTexture, int width, int height, bool replaceOldTexture)
        {
            if (!_main.Visual.ResizeSettingPanel.EnlargeIfSmallerSwitch.Enable)
            {
                if (width <= 0 || height <= 0)
                {
                    MDebug.Error("Size must not be 0px");
                    return;
                }
                if (width > inputTexture.width || height > inputTexture.height) return;
            }
            try
            {
                string outputPath = AssetDatabase.GetAssetPath(inputTexture);
                Texture2D resizedTexture = ResizeTexture(inputTexture, width, height);
                byte[] resizedBytes = resizedTexture.EncodeToPNG();
                if (replaceOldTexture) System.IO.File.WriteAllBytes(outputPath, resizedBytes);
                else System.IO.File.WriteAllBytes($"{outputPath.Replace(".", "(Copied).")}", resizedBytes);
                //MDebug.Notify($"Image resized and saved as {outputPath}");
            }
            catch (System.Exception e)
            {
                MDebug.Error($"Error resizing image: {e.Message}");
            }
        }

        public Texture2D ResizeTexture(Texture2D inputTexture, int width, int height)
        {
            RenderTexture rt = new RenderTexture(width, height, 24);
            Graphics.Blit(inputTexture, rt);

            RenderTexture.active = rt;
            Texture2D resizedTexture = new Texture2D(width, height);
            resizedTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            resizedTexture.Apply();

            RenderTexture.active = null;
            return resizedTexture;
        }
        #endregion
    }
}
#endif