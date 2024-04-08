#if UNITY_EDITOR
using UnityEditor;

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
}
#endif
