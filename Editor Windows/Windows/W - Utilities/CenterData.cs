using UnityEngine;

namespace YNL.Editors.Windows
{
    [CreateAssetMenu(fileName = "Editor Toolbox Data", menuName = "🔗 YのL/🚧 Editor Toolbox/🚧 Toolbox Data")]
    public class CenterData : ScriptableObject
    {
        public AnimationObjectRenamerSettings AnimationObjectRenamer = new();
    }

    [System.Serializable]
    public class AnimationObjectRenamerSettings
    {
        public bool IsAutomaticOn = false;
    }
}