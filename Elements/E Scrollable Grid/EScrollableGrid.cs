#if UNITY_EDITOR
using UnityEngine.UIElements;
using YNL.Editor.Utility;

namespace YNL.Editor.UIElement
{
    public class EScrollableGrid : VisualElement
    {
        private const string _styleSheet = "Style Sheets/Elements/EScrollableGrid";

        public ScrollView Scroll;
        public VisualElement Grid;

        public EScrollableGrid() : base()
        {
            Grid = new VisualElement().AddClass("Grid");
            Scroll = new ScrollView().AddClass("Scroll").AddElements(Grid);

            this.AddStyle(_styleSheet).AddClass("Main").AddElements(Scroll);
        }

        public void AddItems(params VisualElement[] items) => Grid.AddElements(items);
        public void ClearItems()
        {
            Grid.RemoveAllElements();
        }
    }
}
#endif