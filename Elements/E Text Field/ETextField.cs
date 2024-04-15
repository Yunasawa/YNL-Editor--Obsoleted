#if UNITY_EDITOR
using UnityEngine.UIElements;
using YNL.Editor.Utility;

namespace YNL.Editor.UIElement
{
    public class ETextField : Image
    {
        private const string USS_StyleSheet = "Style Sheets/Elements/ETextField";

        private const string _uss_field = "field";
        private const string _uss_color = "color";
        private const string _uss_clip = "clip";


        public TextField Field;

        public ETextField(string label = "") : base()
        {
            this.AddStyle(USS_StyleSheet, EAddress.USSFont).SetName("Root");

            Field = new TextField(label).SetName("Field");
            this.AddElements(Field);

            Field.RegisterValueChangedCallback(OnTextChange);
        }

        private void OnTextChange(ChangeEvent<string> evt)
        {
        }
    }
}
#endif