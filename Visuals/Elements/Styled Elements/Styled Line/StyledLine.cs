#if UNITY_EDITOR
using UnityEngine.UIElements;
using YNL.EditorsObsoleted.Extensions;
using YNL.Extensions.Methods;

namespace YNL.EditorsObsoleted.UIElements.Styled
{
    public class StyledLine : Image
    {
        private const string USS_StyleSheet = "Style Sheets/Elements/Styled/StyledLine";

        public StyledLine(ELineMode mode) : base()
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