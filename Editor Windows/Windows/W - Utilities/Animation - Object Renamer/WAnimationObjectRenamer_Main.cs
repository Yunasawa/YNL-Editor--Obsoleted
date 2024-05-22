#if UNITY_EDITOR
using UnityEditor;
using YNL.Editors.Windows.Utilities;
using YNL.Editors.UIElements.Styled;

namespace YNL.Editors.Windows.Animation.ObjectRenamer
{
    [System.Serializable]
    public class WAnimationObjectRenamer_Main : IMain
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
