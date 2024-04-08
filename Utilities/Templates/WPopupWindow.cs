#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class WPopupWindow<T> : EditorWindow where T : WPopupWindow<T>
{
    private static T _instance;

    public static void Open(params object[] parameters)
    {
        T window = CreateInstance<T>();
        if (!_instance.IsNull())
        {
            if (!_instance != window) _instance.Close();
        }
        _instance = window;

        Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        _instance.position = new Rect(mousePos.x, mousePos.y, 300, 200);

        _instance.ShowPopup();
        _instance.Initialize(parameters);
    }

    private void OnLostFocus()
    {
        _instance.Close();
    }

    protected virtual void Initialize(params object[] parameters) { }

    protected virtual void CreateVirtual() { }
}
#endif