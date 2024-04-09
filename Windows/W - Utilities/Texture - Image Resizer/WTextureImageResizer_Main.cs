#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using YNL.Editor.UIElement;

namespace YNL.Editor.Window.Texture.ImageResizer
{
    [System.Serializable]
    public class WTextureImageResizer_Main : IWindow
    {
        #region ▶ Editor Properties
        public EditorWindow Root;

        public WTextureImageResizer_Visual Visual;
        public WTextureImageResizer_Handler Handler;
        #endregion
        #region ▶ Actions
        public static Action<UnityEngine.Object[]> OnAddImage;
        public static Action<Texture2D> OnRemoveImage;
        #endregion

        public WTextureImageResizer_Main(EditorWindow root, EWindowTagPanel tagPanel)
        {
            Root = root;

            Handler = new(this);
            Visual = new(tagPanel, this);
        }

        public void OnGUI()
        {
            Handler.OnGUI();
        }

        public void OnSelectionChange()
        {
        }
    }
}
#endif