#if UNITY_EDITOR
using UnityEngine.UIElements;
using YNL.EditorsObsoleted.Extensions;

namespace YNL.EditorsObsoleted.Windows.AnimationObjectRenamer
{
    public class Instruction : PopupWindow<Instruction>
    {
        private const string _styleSheet = "Style Sheets/Windows/General Toolbox/Animation - Object Renamer/Instruction";

        public ScrollView Scroll;
        public Image Image;

        public static void Open()
        {
            Show().CloseOnLostFocus().SetSize(660, 500).SetAnchor(true, PopupPivot.BottomLeft);
        }

        protected override void CreateUI()
        {
            this.rootVisualElement.AddStyle(_styleSheet).AddClass("Main");

            Image = new Image().AddClass("Image");

            Scroll = new ScrollView().AddClass("Scroll").AddElements(Image);

            this.rootVisualElement.AddElements(Scroll);
        }
    }
}
#endif