#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
        }
    }
}
#endif