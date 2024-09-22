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
        public AORSettings AnimationObjectRenamer = new();

        [ContextMenu("Clear Logs")]
        public void ClearLogs()
        {
            AnimationObjectRenamer.AutomaticLogs.Clear();
            Visual.RefreshLogPanel();
        }
    }

    [System.Serializable]
    public class AORSettings
    {
        private static CenterData _centerData;

        public bool IsAutomaticOn = false;
        public List<AutomaticLog> AutomaticLogs = new();

        [System.Serializable]
        public enum Event { RenameSingle, RenameMultiple, MoveSucceed, MoveConflict, MoveOutbound, Destroy }

        [System.Serializable]
        public struct AutomaticLog
        {
            public Event Event;
            public bool IsSucceeded;
            public string CurrentTime;
            public string[] OldNames;
            public string[] NewNames;

            public AutomaticLog(Event @event, bool isSucceeded, string[] oldNames, string[] newNames)    
            {
                Event = @event;
                IsSucceeded = isSucceeded;
                string original = DateTime.Now.ToString();
                int spaceIndex = original.IndexOf(' ');
                CurrentTime = $"<color=#96ffdc>{original.Substring(0, spaceIndex)}</color>" + "\n" + original.Substring(spaceIndex + 1);
                OldNames = oldNames;
                NewNames = newNames;
            }
        }
    }
}
#endif