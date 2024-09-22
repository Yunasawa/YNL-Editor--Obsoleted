#if UNITY_EDITOR && YNL_UTILITIES
using UnityEngine.UIElements;
using System;
using YNL.Editors.Extensions;

namespace YNL.Editors.UIElements.Plained
{
    public class PlainedEllipsisButton : Button
    {
        private const string _styleSheet = "Style Sheets/Elements/Plained/PlainedEllipsisButton";

        public PlainedEllipsisButton() : base(null) => Initialize();
        public PlainedEllipsisButton(Action action) : base(action) => Initialize();

        private void Initialize()
        {
            this.AddStyle(_styleSheet).AddClass("Main");
        }
    }
}
#endif