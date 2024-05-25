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
    }
}