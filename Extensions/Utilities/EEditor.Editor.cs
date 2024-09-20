using System.Reflection;
using UnityEngine.UIElements;

namespace YNL.Editors.Extensions
{
    public static partial class EEditor
    {
        public class Editor
        {
            public static VisualElement GetToolbarZone(string zone)
            {
                var toolbarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbar")!;
                var toolbar = toolbarType.GetField("get", BindingFlags.Public | BindingFlags.Static)!.GetValue(null);
                var toolbarRootVisualElement = toolbarType.GetField("m_Root", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(toolbar) as VisualElement;
                return toolbarRootVisualElement.Q(zone);
            }
            public static VisualElement GetToolbarZonePlayMode() => GetToolbarZone("ToolbarZonePlayMode");
            public static VisualElement GetToolbarZoneLeft() => GetToolbarZone("ToolbarZoneLeftAlign");
            public static VisualElement GetToolbarZoneRight() => GetToolbarZone("ToolbarZoneRightAlign");
        }
    }
}