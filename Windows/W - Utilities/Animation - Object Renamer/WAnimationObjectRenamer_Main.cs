#if UNITY_EDITOR
using UnityEditor;
using YNL.Editor.Extension;
using YNL.Editor.UIElement;

namespace YNL.Editor.Window.Animation.ObjectRenamer
{
    [System.Serializable]
    public class WAnimationObjectRenamer_Main : IWindow
    {
        public EditorWindow Root;

        public WAnimationObjectRenamer_Visual Visual;
        public WAnimationObjectRenamer_Handler Handler;

        public WAnimationObjectRenamer_Main(EditorWindow root, EWindowTagPanel tagPanel)
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
            Handler.OnSelectionChange();
        }

        public void OpenInstruction()
        {
            WAnimationObjectRenamer_Instruction.Open(660, 500, WPopupPivot.BottomLeft);
        }
    }
}
#endif
