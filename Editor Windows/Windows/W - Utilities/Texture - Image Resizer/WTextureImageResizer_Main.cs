#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using YNL.Editors.Windows.Utilities;
using YNL.Editors.UIElements.Styled;

namespace YNL.Editors.Windows.Texture.ImageResizer
{
    [System.Serializable]
    public class WTextureImageResizer_Main : IMain
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

        public void OpenInstruction()
        {

            WTextureImageResizer_Instruction.Open(660, 500, WPopupPivot.BottomLeft);
        }
    }
}
#endif