#if UNITY_EDITOR
using UnityEditor;
using YNL.EditorsObsoleted.UIElements.Styled;

namespace YNL.EditorsObsoleted.Windows.AnimationObjectRenamer
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
    }
}
#endif
