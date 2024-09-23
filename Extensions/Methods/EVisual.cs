using UnityEngine.UIElements;

namespace YNL.EditorsObsoleted.Extensions
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