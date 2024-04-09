#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEngine.UIElements;
using YNL.Editor.Extensions;

namespace YNL.Editor.Utilities
{
    public static class EUtilities
    {
        public static string Custom(this string uss, string custom) => $"{uss}__{custom}";
        public static string Hover(this string uss) => $"{uss}__hover";
        public static string Drag(this string uss) => $"{uss}__drag";

        public static VisualElement CustomField<T>(string label, string value, Action<T> valueChanged, params string[] style)
        {
            VisualElement element = null;

            if (typeof(T) == typeof(string))
            {
                element = new TextField(label).AddClass(style);
                (element as TextField).value = value;
                (element as TextField).RegisterValueChangedCallback(evt => valueChanged?.Invoke((T)Convert.ChangeType(evt.newValue, typeof(T))));
            }
            else if (typeof(T) == typeof(int))
            {
                element = new IntegerField(label).AddClass(style);
                (element as IntegerField).value = int.Parse(value);
                (element as IntegerField).RegisterValueChangedCallback(evt => valueChanged?.Invoke((T)Convert.ChangeType(evt.newValue, typeof(T))));
            }
            else if (typeof(T) == typeof(float))
            {
                element = new FloatField(label).AddClass(style);
                (element as FloatField).value = float.Parse(value);
                (element as FloatField).RegisterValueChangedCallback(evt => valueChanged?.Invoke((T)Convert.ChangeType(evt.newValue, typeof(T))));
            }
            else if (typeof(T) == typeof(bool))
            {
                element = new Toggle(label).AddClass(style);
                (element as Toggle).value = bool.Parse(value);
                (element as Toggle).RegisterValueChangedCallback(evt => valueChanged?.Invoke((T)Convert.ChangeType(evt.newValue, typeof(T))));
            }
            else if (typeof(T).IsEnum)
            {
                element = new EnumField(label, (Enum)Activator.CreateInstance(typeof(T))).AddClass(style);
                (element as EnumField).value = (Enum)Convert.ChangeType(EEditorExtension.Parse<T>(value), typeof(Enum));
                (element as EnumField).RegisterValueChangedCallback(evt => valueChanged?.Invoke((T)Convert.ChangeType(evt.newValue, typeof(T))));
            }
            else element = new Image();

            return element;
        }

        public static VisualElement CustomField(FieldInfo fieldInfo, string label, string value, Action<object> valueChanged, params string[] style)
        {
            label = label.AddSpaces();

            VisualElement element = null;

            Type fieldType = fieldInfo.FieldType;

            if (fieldType == typeof(string))
            {
                element = new TextField(label).AddClass(style);
                if (!value.IsNull()) (element as TextField).value = value;
                (element as TextField).RegisterValueChangedCallback(evt => valueChanged?.Invoke(evt.newValue));
            }
            else if (fieldType == typeof(int))
            {
                element = new IntegerField(label).AddClass(style);
                if (!value.IsNull()) (element as IntegerField).value = int.Parse(value);
                (element as IntegerField).RegisterValueChangedCallback(evt => valueChanged?.Invoke(evt.newValue));
            }
            else if (fieldType == typeof(float))
            {
                element = new FloatField(label).AddClass(style);
                if (!value.IsNull()) (element as FloatField).value = float.Parse(value);
                (element as FloatField).RegisterValueChangedCallback(evt => valueChanged?.Invoke(evt.newValue));
            }
            else if (fieldType == typeof(bool))
            {
                element = new Toggle(label).AddClass(style);
                if (!value.IsNull()) (element as Toggle).value = bool.Parse(value);
                (element as Toggle).RegisterValueChangedCallback(evt => valueChanged?.Invoke(evt.newValue));
            }
            else if (fieldType.IsEnum)
            {
                element = new EnumField(label, (Enum)Activator.CreateInstance(fieldType)).AddClass(style);
                if (!value.IsNull()) (element as EnumField).value = (Enum)Enum.Parse(fieldType, value);
                (element as EnumField).RegisterValueChangedCallback(evt => valueChanged?.Invoke(evt.newValue));
            }
            else
            {
                MDebug.Caution($"Unsupported field type: {fieldType}");
            }

            return element;
        }
    }
}
#endif