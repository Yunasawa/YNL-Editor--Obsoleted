#if UNITY_EDITOR
using UnityEngine.UIElements;
using YNL.Editor.Utilities;

namespace YNL.Editor.UIElement
{
    public class ELine : Image
    {
        private const string USS_StyleSheet = "Style Sheets/Elements/ELine";

        public ELine(ELineMode mode) : base()
        {
            this.AddStyle(USS_StyleSheet);
            if (mode == ELineMode.Horizontal) this.SetName("Horizontal");
            if (mode == ELineMode.Vertical) this.SetName("Vertical");
        }
    }

    public enum ELineMode
    {
        Vertical, Horizontal
    }
}
#endif