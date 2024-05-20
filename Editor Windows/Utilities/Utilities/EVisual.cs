using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace YNL.Editors.Windows.Utilities
{
    public static class EVisual
    {
        public static Label AddLabel(this VisualElement container, string text, string style = null)
        {
            Label label = new Label();
            label.text = text;

            label.AddToClassList(style);

            container.Add(label);
            return label;
        }

        public static Button AddButton(this VisualElement container, string title, string style = null)
        {
            Button button = new Button();
            button.text = title;
            //button.style.justifyContent = Justify.Center;
            button.AddToClassList(style);

            container.Add(button);
            return button;
        }

        public static Toggle AddToggle(this VisualElement container, string title, string style = null)
        {
            Toggle toggle = new Toggle();
            toggle.text = title;

            toggle.AddToClassList(style);

            container.Add(toggle);
            return toggle;
        }
    }

    public static class WorkaroundUnityUIToolkitBrokenObjectSelector
    {
        public static void ShowObjectPicker<T>(this T initialValue, Action<T> OnSelectorClosed, Action<T> OnSelectionChanged) where T : UnityEngine.Object
        {
            ShowObjectPicker<T>(OnSelectorClosed, OnSelectionChanged, initialValue);
        }

        public static void ShowObjectPicker<T>(Action<T> OnSelectorClosed, Action<T> OnSelectionChanged, T initialValueOrNull = null) where T : UnityEngine.Object
        {
            var hiddenType = typeof(Editor).Assembly.GetType("UnityEditor.ObjectSelector");
            var ps = hiddenType.GetProperties(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty);
            PropertyInfo piGet = hiddenType.GetProperty("get", BindingFlags.Public | BindingFlags.Static);
            var os = piGet.GetValue(null);

            MethodInfo miShow = hiddenType.GetMethod("Show", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[]
            {
            typeof(T),
            typeof(    System.Type),
            typeof(UnityEngine.Object),
            typeof(bool),
            typeof(List<int>),
            typeof(Action<UnityEngine.Object>),
            typeof(Action<UnityEngine.Object>)
            }, new ParameterModifier[0]);
            //Action<UnityEngine.Object> onSelectorClosed = o => { Debug.Log( "selector closed"+o.name ); };
            //Action<UnityEngine.Object> onSelectedUpdated = o => { Debug.Log( "selector updated"+o.name ); };
            miShow.Invoke(os, new object[]
                {
                initialValueOrNull,
                typeof(T),
                null,
                false,
                null,
                OnSelectorClosed,
                OnSelectionChanged,
                }
            );
        }
    }
}