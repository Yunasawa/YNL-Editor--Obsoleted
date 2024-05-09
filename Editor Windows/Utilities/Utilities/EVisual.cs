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
}