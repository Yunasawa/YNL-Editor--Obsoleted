#if UNITY_EDITOR && YNL_UTILITIES
using UnityEngine.UIElements;
using YNL.EditorsObsoleted.Extensions;
using YNL.Extensions.Methods;

namespace YNL.EditorsObsoleted.UIElements.Flex
{
    public class FlexElementGroup : VisualElement
    {
        private const string _styleSheet = "Style Sheets/Elements/Flex/Flex Element Group/FlexElementGroup";

        public FlexElementGroup(params VisualElement[] elements)
        {
            this.AddStyle(_styleSheet).AddClass("Main");

            this.AddElements(elements);  
        }
    }
}
#endif