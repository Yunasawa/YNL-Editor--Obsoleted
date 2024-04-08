#if UNITY_EDITOR
using System.Reflection;
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using YNL.Editor.Utilities;
using UnityEngine.UIElements;

public class WMessagePopup : EditorWindow
{
    private const string _styleSheet = "Style Sheets/Windows/W - Message Popup/WMessagePopup";

    private static string _message = "This is not available";
    private static GUIStyle messageStyle = null;

    private double _currentTime;

    public Label Message;

    public static void Show(params object[] parameters)
    {
        WMessagePopup window = CreateInstance<WMessagePopup>();
        window.ShowPopup();
        window.Initialize(parameters);
    }

    protected void Initialize(params object[] parameters)
    {
        _message = (string)parameters[0];

        Message = new Label(_message).AddClass("Message");

        if (messageStyle == null)
        {
            messageStyle = new GUIStyle(EditorStyles.label)
            {
                wordWrap = true,
                alignment = TextAnchor.MiddleCenter
            };
        }

        Vector2 contentSize = messageStyle.CalcSize(new GUIContent(_message));

        this.minSize = this.maxSize = new Vector2(contentSize.x + 60, contentSize.y + 10);

        Rect mainUnityWindowRect = Extensions.GetEditorMainWindowPos();
        this.position = new Rect(
            (mainUnityWindowRect.width - this.minSize.x) / 2,
            (mainUnityWindowRect.height - this.minSize.y) / 2,
            this.minSize.x,
            this.minSize.y
        );

        _currentTime = EditorApplication.timeSinceStartup + 2f;
        EditorApplication.update += OnEditorUpdate;

        CreateVirtual();
    }

    protected void CreateVirtual()
    {
        Message = new Label(_message).AddClass("Message");
        this.rootVisualElement.AddStyle(_styleSheet, EAddress.USSFont).AddClass("Main").AddElements(Message);
    }

    void OnEditorUpdate()
    {
        if (EditorApplication.timeSinceStartup > _currentTime)
        {
            EditorApplication.update -= OnEditorUpdate;
            this.Close();
        }
    }
}

public static class Extensions
{
    public static Rect GetEditorMainWindowPos()
    {
        var containerWinType = AppDomain.CurrentDomain.GetAllDerivedTypes(typeof(ScriptableObject)).Where(t => t.Name == "ContainerWindow").FirstOrDefault();
        if (containerWinType == null)
            throw new MissingMemberException("Can't find internal type ContainerWindow. Maybe something has changed inside Unity");

        var showModeField = containerWinType.GetField("m_ShowMode", BindingFlags.NonPublic | BindingFlags.Instance);
        var positionProperty = containerWinType.GetProperty("position", BindingFlags.Public | BindingFlags.Instance);
        if (showModeField == null || positionProperty == null)
            throw new MissingFieldException("Can't find internal fields 'm_ShowMode' or 'position'. Maybe something has changed inside Unity");

        var windows = Resources.FindObjectsOfTypeAll(containerWinType);
        foreach (var win in windows)
        {
            var showmode = (int)showModeField.GetValue(win);
            if (showmode == 4) // main window
            {
                var pos = (Rect)positionProperty.GetValue(win, null);
                return pos;
            }
        }
        throw new NotSupportedException("Can't find internal main window. Maybe something has changed inside Unity");
    }

    public static IEnumerable<Type> GetAllDerivedTypes(this AppDomain appDomain, Type type)
    {
        var allAssemblies = appDomain.GetAssemblies();
        var allTypes = allAssemblies.SelectMany(a => a.GetTypes());
        var derivedTypes = allTypes.Where(t => type.IsAssignableFrom(t) && t != type);

        return derivedTypes;
    }
}
#endif