#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using YNL.Editors.Windows.Utilities;
using YNL.Editors.UIElements;

namespace YNL.Editors.Windows.Texture.ImageInverter
{
    [System.Serializable]
    public class WTextureImageInverter_Main : IMain
    {
        #region ▶ Editor Properties
        public EditorWindow Root;

        public WTextureImageInverter_Visual Visual;
        public WTextureImageInverter_Handler Handler;
        #endregion
        #region ▶ Actions
        public static Action<UnityEngine.Object[]> OnAddImage;
        public static Action<Texture2D> OnRemoveImage;
        #endregion

        public WTextureImageInverter_Main(EditorWindow root, EWindowTagPanel tagPanel)
        {
            Root = root;

            Handler = new(this);
            Visual = new(tagPanel, this);
        }

        public void OpenInstruction()
        {
            WTextureImageInverter_Instruction.Open(660, 500, WPopupPivot.BottomLeft);
        }
    }
}
#endif