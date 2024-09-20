using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using YNL.Extensions.Methods;

namespace YNL.Editors.Windows.AnimationObjectRenamer
{
    public class Variable
    {
        private static CenterData _centerDataGetter;
        public static CenterData CenterData
        {
            get
            {
                if (_centerDataGetter.IsNull())
                {
                    _centerDataGetter = "Editor Toolbox Data".LoadResource<CenterData>();
                    return _centerDataGetter;
                }
                else return _centerDataGetter;
            }
        }

        public static bool IsAutomaticPanel = false;
        public static bool IsAutomaticOn
        {
            get => CenterData.AnimationObjectRenamer.IsAutomaticOn;
            set
            {
                CenterData.AnimationObjectRenamer.IsAutomaticOn = value;
                EditorUtility.SetDirty(CenterData);
                AssetDatabase.SaveAssets();
            }
        }
        public static List<AORSettings.AutomaticLog> AutomaticLogs
            => CenterData.AnimationObjectRenamer?.AutomaticLogs;

        public static Action OnModeChanged;

        public static void SaveData()
        {
            EditorUtility.SetDirty(CenterData);
            AssetDatabase.SaveAssets();
        }
    }
}