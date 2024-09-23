#if UNITY_EDITOR && YNL_UTILITIES
using UnityEngine.UIElements;
using System;
using YNL.EditorsObsoleted.Extensions;

namespace YNL.EditorsObsoleted.UIElements.Plained
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