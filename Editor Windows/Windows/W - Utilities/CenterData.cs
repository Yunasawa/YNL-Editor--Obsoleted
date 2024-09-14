#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using YNL.Editors.Windows.AnimationObjectRenamer;

namespace YNL.Editors.Windows
{
    [CreateAssetMenu(fileName = "Editor Toolbox Data", menuName = "🔗 YのL/🚧 Editor Toolbox/🚧 Toolbox Data")]
    public class CenterData : ScriptableObject
    {
        public WindowType CurrentWindow;
        public AnimationObjectRenamerSettings AnimationObjectRenamer = new();

        [ContextMenu("Clear Logs")]
        public void ClearLogs()
        {
            AnimationObjectRenamer.AutomaticLogs.Clear();
            Visual.RefreshLogPanel();
        }
    }

    [System.Serializable]
    public class AnimationObjectRenamerSettings
    {
        private static CenterData _centerData;

        public bool IsAutomaticOn = false;
        public List<AutomaticLog> AutomaticLogs = new();

        [System.Serializable]
        public struct AutomaticLog
        {
            public bool IsSucceeded;
            public string CurrentTime;
            public string Name;
            public string Path;
            public int AnimatorAmount;
            public int ClipAmount;

            public GameObject BindedObject;

            public AutomaticLog(bool isSucceeded, string name, string path, int animatorAmount, int clipAmount, GameObject bindedObject)    
            {
                IsSucceeded = isSucceeded;
                string original = DateTime.Now.ToString();
                int spaceIndex = original.IndexOf(' ');
                CurrentTime = $"<color=#96ffdc>{original.Substring(0, spaceIndex)}</color>" + "\n" + original.Substring(spaceIndex + 1);
                Name = name;
                Path = path;
                AnimatorAmount = animatorAmount;
                ClipAmount = clipAmount;
                BindedObject = bindedObject;
            }
        }
    }
}
#endif