using UnityEngine.UIElements;

namespace YNL.Editors.Windows
{
    public abstract class EVisual : VisualElement
    {
        protected string _windowIcon;
        protected string _windowTitle;
        protected string _windowSubtitle;

        public void SetWindowTitle(string icon, string title, string subtitle)
        {
            _windowIcon = icon;
            _windowTitle = title;
            _windowSubtitle = subtitle;
        }
    }
}
