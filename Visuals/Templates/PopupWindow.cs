#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using YNL.Extensions.Methods;

namespace YNL.Editors.Windows
{

    public class PopupWindow<T> : EditorWindow where T : PopupWindow<T>
    {
        protected static T _instance;
        protected bool _closeOnLostFocus = false;
        protected bool _isMouseAnchor = false;

        private Vector2 _size;

        static PopupWindow()
        {
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
        }

        private static void OnBeforeAssemblyReload()
        {
            if (_instance != null)
            {
                _instance.Close();
            }
        }

        public static T Show(params object[] parameters)
        {
            T[] windows = Resources.FindObjectsOfTypeAll<T>();

            foreach (var window in windows) window.Close();

            _instance = CreateInstance<T>();

            _instance.ShowPopup();
            _instance.Initialize(parameters);
            _instance.CreateUI();

            return _instance;
        }

        private void OnLostFocus()
        {
            if (!_closeOnLostFocus) return;

            _instance.Close();
        }

        protected virtual void Initialize(params object[] parameters) { }
        protected virtual void CreateUI() { }

        public T CloseOnLostFocus()
        {
            _closeOnLostFocus = true;
            return _instance;
        }
        public T SetSize(int width, int height)
        {
            _size.x = width;
            _size.y = height;

            this.minSize = this.maxSize = _size;

            return _instance;
        }
        public T SetAnchor(bool isMouseAnchor = false, PopupPivot pivot = PopupPivot.TopLeft)
        {
            _isMouseAnchor = isMouseAnchor;

            if (!_isMouseAnchor)
            {
                var main = EditorGUIUtility.GetMainWindowPosition();
                var pos = position;
                pos.x = main.x + (main.width - pos.width) * 0.5f;
                pos.y = main.y + (main.height - pos.height) * 0.5f;
                position = pos;
            }
            else
            {
                Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                if (pivot == PopupPivot.TopLeft) _instance.position = new Rect(mousePos.x, mousePos.y, _size.x, _size.y);
                if (pivot == PopupPivot.BottomLeft) _instance.position = new Rect(mousePos.x, mousePos.y - _size.y, _size.x, _size.y);
                if (pivot == PopupPivot.TopRight) _instance.position = new Rect(mousePos.x - _size.x, mousePos.y, _size.x, _size.y);
                if (pivot == PopupPivot.BottomRight) _instance.position = new Rect(mousePos.x - _size.x, mousePos.y - _size.y, _size.x, _size.y);
            }

            return _instance;
        }

        private void CenterOnScreen2()
        {
            var main = EditorGUIUtility.GetMainWindowPosition();
            var pos = position;
            pos.x = main.x + (main.width - pos.width) * 0.5f;
            pos.y = main.y + (main.height - pos.height) * 0.5f;
            position = pos;
        }
    }

    public enum PopupPivot
    {
        TopLeft, BottomLeft, TopRight, BottomRight
    }
}
#endif