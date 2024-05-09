#if UNITY_EDITOR
using UnityEngine.UIElements;
using YNL.Editors.Windows.Utilities;
using YNL.Editors.Windows;

namespace YNL.Editors.Windows.Animation.ObjectRenamer
{
    public class WAnimationObjectRenamer_Instruction : WPopupWindow<WAnimationObjectRenamer_Instruction>
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