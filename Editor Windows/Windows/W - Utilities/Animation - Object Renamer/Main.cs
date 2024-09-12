#if UNITY_EDITOR
using UnityEditor;
using YNL.Extensions.Methods;
using YNL.Editors.UIElements.Styled;

namespace YNL.Editors.Windows.AnimationObjectRenamer
{
    [System.Serializable]
    public class Main : IMain
    {
        public EditorWindow Root;

        public Visual Visual;
        public Handler Handler;

        public Main(EditorWindow root, StyledWindowTagPanel tagPanel)
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
            Instruction.Open(660, 500, WPopupPivot.BottomLeft);
        }
    }
}
#endif
