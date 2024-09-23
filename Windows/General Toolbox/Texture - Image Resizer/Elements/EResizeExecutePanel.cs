#if UNITY_EDITOR
using UnityEngine.UIElements;
using YNL.EditorsObsoleted.UIElements.Styled;
using YNL.EditorsObsoleted.Extensions;
using YNL.Extensions.Methods;

namespace YNL.EditorsObsoleted.Windows.TextureImageResizer
{
    public class EResizeExecutePanel : Button
    {
        private const string _styleSheet = "Style Sheets/Windows/General Toolbox/Texture - Image Resizer/EResizeExecutePanel";

        public Button Execute;
        public StyledSwitchToggle ReplaceOldImageSwitch;
        public Image ReplaceOldImagePanel;
        public StyledSwitchToggle SaveAsACopySwitch;
        public Image SaveAsACopyPanel;

        public bool IsReplaceOldImage = true;

        public EResizeExecutePanel() : base()
        {
            this.AddStyle(_styleSheet, ESheet.Font).AddClass("Main");

            ReplaceOldImageSwitch = new StyledSwitchToggle(false).AddClass("ReplaceOldImageSwitch");
            ReplaceOldImageSwitch.OnSwitch += (enable) => SwitchSaveType(enable);
            ReplaceOldImagePanel = new Image().AddClass("ReplaceOldImagePanel").AddElements(new Label("Replace old image").AddClass("Label"), ReplaceOldImageSwitch);

            SaveAsACopySwitch = new StyledSwitchToggle(true).AddClass("SaveAsACopySwitch");
            SaveAsACopySwitch.OnSwitch += (enable) => SwitchSaveType(!enable);
            SaveAsACopyPanel = new Image().AddClass("SaveAsACopyPanel").AddElements(new Label("Save as a copy").AddClass("Label"), SaveAsACopySwitch);

            Execute = new Button().AddClass("Execute").SetText("Resize");

            this.AddElements(ReplaceOldImagePanel, SaveAsACopyPanel, Execute);

            SwitchSaveType(true);
        }

        public void SwitchSaveType(bool enable)
        {
            IsReplaceOldImage = enable;

            ReplaceOldImageSwitch.SetEnable(IsReplaceOldImage);
            SaveAsACopySwitch.SetEnable(!IsReplaceOldImage);
        }
    }
}
#endif