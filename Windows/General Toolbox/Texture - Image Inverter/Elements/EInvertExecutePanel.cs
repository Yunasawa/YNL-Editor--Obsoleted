#if UNITY_EDITOR
using UnityEngine.UIElements;
using YNL.Editors.UIElements.Styled;
using YNL.Editors.Extensions;

namespace YNL.Editors.Windows.TextureImageInverter
{
    public class EInvertExecutePanel : Button
    {
        private const string _styleSheet = "Style Sheets/Windows/W - Utilities/Texture - Image Inverter/EInvertExecutePanel";

        public StyledSwitchToggle ReplaceOldImageSwitch;
        public Image ReplaceOldImagePanel;
        public StyledSwitchToggle SaveAsACopySwitch;
        public Image SaveAsACopyPanel;

        public Image Icon;

        public Button Execute;

        public bool IsReplaceOldImage = true;

        public EInvertExecutePanel() : base()
        {
            this.AddStyle(_styleSheet, ESheet.Font).AddClass("Main");

            ReplaceOldImageSwitch = new StyledSwitchToggle(false).AddClass("ReplaceOldImageSwitch");
            ReplaceOldImageSwitch.OnSwitch += (enable) => SwitchSaveType(enable);
            ReplaceOldImagePanel = new Image().AddClass("ReplaceOldImagePanel").AddElements(new Label("Replace old image").AddClass("Label"), ReplaceOldImageSwitch);

            SaveAsACopySwitch = new StyledSwitchToggle(true).AddClass("SaveAsACopySwitch");
            SaveAsACopySwitch.OnSwitch += (enable) => SwitchSaveType(!enable);
            SaveAsACopyPanel = new Image().AddClass("SaveAsACopyPanel").AddElements(new Label("Save as a copy").AddClass("Label"), SaveAsACopySwitch);

            Icon = new Image().AddClass("InvertIcon");

            Execute = new Button().AddClass("Execute").SetText("Invert");

            this.AddElements(ReplaceOldImagePanel, SaveAsACopyPanel, Icon, Execute);

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