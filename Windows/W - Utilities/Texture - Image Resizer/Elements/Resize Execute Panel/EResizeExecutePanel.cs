﻿#if UNITY_EDITOR
using UnityEngine.UIElements;
using YNL.Editor.Utilities;

namespace YNL.Editor.Window.Texture.ImageResizer
{
    public class EResizeExecutePanel : Button
    {
        private const string _styleSheet = EAddress.FolderPath + "Windows/W - Utilities/Texture - Image Resizer/Elements/Resize Execute Panel/EResizeExecutePanel.uss";
        private const string _texturePath = EAddress.FolderPath + "Textures/Windows/Texture Center/";

        public Button Execute;
        public ESwitchToggle ReplaceOldImageSwitch;
        public Image ReplaceOldImagePanel;
        public ESwitchToggle SaveAsACopySwitch;
        public Image SaveAsACopyPanel;

        public bool IsReplaceOldImage = true;

        public EResizeExecutePanel() : base()
        {
            this.AddStyle(_styleSheet, EAddress.USSFont).AddClass("Main");

            ReplaceOldImageSwitch = new ESwitchToggle(false).AddClass("ReplaceOldImageSwitch");
            ReplaceOldImageSwitch.OnSwitch += (enable) => SwitchSaveType(enable);
            ReplaceOldImagePanel = new Image().AddClass("ReplaceOldImagePanel").AddElements(new Label("Replace old image").AddClass("Label"), ReplaceOldImageSwitch);

            SaveAsACopySwitch = new ESwitchToggle(true).AddClass("SaveAsACopySwitch");
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