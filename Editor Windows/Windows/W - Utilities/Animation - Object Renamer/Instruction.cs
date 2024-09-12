#if UNITY_EDITOR
using UnityEngine.UIElements;
using YNL.Editors.Windows.Utilities;

namespace YNL.Editors.Windows.AnimationObjectRenamer
{
    public class Instruction : WPopupWindow<Instruction>
    {
        private const string _styleSheet = "Style Sheets/Windows/W - Utilities/Animation - Object Renamer/WAnimationObjectRenamer_Popup";

        public ScrollView Scroll;
        public Image Image;

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