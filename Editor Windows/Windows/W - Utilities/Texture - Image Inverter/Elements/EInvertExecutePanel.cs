#if UNITY_EDITOR
using UnityEngine.UIElements;
using YNL.Editors.UIElements;
using YNL.Editors.Windows.Utilities;

namespace YNL.Editors.Windows.Texture.ImageInverter
{
    public class EInvertExecutePanel : Button
    {
        private const string _styleSheet = "Style Sheets/Windows/W - Utilities/Texture - Image Inverter/EInvertExecutePanel";

        public ESwitchToggle ReplaceOldImageSwitch;
        public Image ReplaceOldImagePanel;
        public ESwitchToggle SaveAsACopySwitch;
        public Image SaveAsACopyPanel;

        public Image Icon;

        public Button Execute;

        public bool IsReplaceOldImage = true;

        public EInvertExecutePanel() : base()
        {
            this.AddStyle(_styleSheet, EAddress.USSFont).AddClass("Main");

            ReplaceOldImageSwitch = new ESwitchToggle(false).AddClass("ReplaceOldImageSwitch");
            ReplaceOldImageSwitch.OnSwitch += (enable) => SwitchSaveType(enable);
            ReplaceOldImagePanel = new Image().AddClass("ReplaceOldImagePanel").AddElements(new Label("Replace old image").AddClass("Label"), ReplaceOldImageSwitch);

            SaveAsACopySwitch = new ESwitchToggle(true).AddClass("SaveAsACopySwitch");
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