#if UNITY_EDITOR && YNL_UTILITIES
using UnityEditor;
using YNL.Extensions.Methods;

namespace YNL.Editors.Extensions
{
    public static partial class EEditor
    {
        public class Layer
        {
            private static int maxLayers = 31;

            private static SerializedObject _tagManager;
            private static SerializedProperty _layerProperty;

            private static void GetLayerProperty()
            {
                if (_tagManager == null) _tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/tagManager.asset")[0]);
                if (_layerProperty == null) _layerProperty = _tagManager.FindProperty("layers");
            }
            private static bool PropertyExists(SerializedProperty property, int start, int end, string value)
            {
                for (int i = start; i < end; i++)
                {
                    SerializedProperty t = property.GetArrayElementAtIndex(i);
                    if (t.stringValue.Equals(value)) return true;
                }
                return false;
            }

            public static bool LayerExists(string layerName)
            {
                GetLayerProperty();
                return PropertyExists(_layerProperty, 0, maxLayers, layerName);
            }
            public static void AddLayer(string name)
            {
                if (LayerExists(name)) return;
                if (name != null || name != "") CreateLayer(name);
            }
            public static void RemoveLayer(string name)
            {
                if (!LayerExists(name)) return;
                DeleteLayer(name);
            }
            public static bool CreateLayer(string layerName)
            {
                GetLayerProperty();

                if (!PropertyExists(_layerProperty, 0, maxLayers, layerName))
                {
                    SerializedProperty sp;
                    for (int i = 6, j = maxLayers; i < j; i++)
                    {
                        sp = _layerProperty.GetArrayElementAtIndex(i);
                        if (sp.stringValue == "")
                        {
                            sp.stringValue = layerName;
                            MDebug.Notify($"Layer <color=#7aff7a><b>{layerName}</b></color> has been added");
                            _tagManager.ApplyModifiedProperties();
                            return true;
                        }
                        if (i == j)
                        {
                            MDebug.Notify("All allowed layers have been filled");
                        }
                    }
                }
                else
                {
                    MDebug.Notify($"Layer <color=#7aff7a><b>{layerName}</b></color> already exists");
                }
                return false;
            }
            public static bool DeleteLayer(string layerName)
            {
                GetLayerProperty();

                if (PropertyExists(_layerProperty, 0, _layerProperty.arraySize, layerName))
                {
                    SerializedProperty sp;

                    for (int i = 0, j = _layerProperty.arraySize; i < j; i++)
                    {

                        sp = _layerProperty.GetArrayElementAtIndex(i);

                        if (sp.stringValue == layerName)
                        {
                            sp.stringValue = "";
                            MDebug.Notify($"Layer <color=#7aff7a><b>{layerName}</b></color> has been removed");
                            _tagManager.ApplyModifiedProperties();
                            return true;
                        }

                    }
                }
                return false;
            }
        }
    }
}
#endif