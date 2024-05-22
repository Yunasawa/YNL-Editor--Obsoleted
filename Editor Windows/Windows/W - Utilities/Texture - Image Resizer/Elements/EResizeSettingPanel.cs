﻿#if UNITY_EDITOR
using System;
using UnityEngine.UIElements;
using YNL.Editors.Windows.Utilities;
using YNL.Editors.UIElements.Styled;

namespace YNL.Editors.Windows.Texture.ImageResizer
{
    public class EResizeSettingPanel : Button
    {
        private const string _styleSheet = "Style Sheets/Windows/W - Utilities/Texture - Image Resizer/EResizeSettingPanel";
        private const string _texturePath = "Textures/Windows/Texture Center/Image Resizer/";

        public Image TagPanel;
        public Button PixelResizeTag;
        public Button PercentResizeTag;

        public VisualElement PixelSetting;
        public Image WidthPanel;
        public TextField Width;
        public Image HeightPanel;
        public TextField Height;
        public ESwitchToggle EnlargeIfSmallerSwitch;
        public Image EnlargeIfSmallerPanel;
        public ESwitchToggle KeepAspectRatioSwitch;
        public Image KeepAspectRatioPanel;

        public VisualElement PercentSetting;

        public Action<int> OnWidthChaned;
        public Action<int> OnHeightChaned;

        public EResizeSettingPanel() : base()
        {
            this.AddStyle(_styleSheet, EStyleSheet.Font).AddClass("Main");

            PixelResizeTag = new Button().AddClass("Tag").SetBackgroundImage(_texturePath + "Pixel Resize");
            PixelResizeTag.clicked += () => SwitchTag(true);
            PercentResizeTag = new Button().AddClass("Tag").SetBackgroundImage(_texturePath + "Percent Resize");
            PercentResizeTag.clicked += () => SwitchTag(false);

            Width = new TextField().AddClass("Size", "Width").SetText("...");
            Width.RegisterValueChangedCallback(WidthChanged);
            WidthPanel = new Image().AddClass("SizePanel").AddElements(new Label("Width (px)").AddClass("SizeLabel"), Width);
            
            Height = new TextField().AddClass("Size", "Height").SetText("...");
            Height.RegisterValueChangedCallback(HeightChanged);
            HeightPanel = new Image().AddClass("SizePanel").AddElements(new Label("Height (px)").AddClass("SizeLabel"), Height);

            EnlargeIfSmallerSwitch = new ESwitchToggle(false).AddClass("EnlargeIfSmallerSwitch");
            EnlargeIfSmallerPanel = new Image().AddClass("EnlargeIfSmallerPanel").AddElements(new Label("Enlarge if smaller").AddClass("SizeLabel"), EnlargeIfSmallerSwitch);

            KeepAspectRatioSwitch = new ESwitchToggle(true).AddClass("KeepAspectRatioSwitch");
            KeepAspectRatioPanel = new Image().AddClass("KeepAspectRatioPanel").AddElements(new Label("Keep aspect ratio").AddClass("SizeLabel"), KeepAspectRatioSwitch);

            PixelSetting = new VisualElement().AddClass("PixelSetting").AddElements(WidthPanel, HeightPanel, EnlargeIfSmallerPanel, KeepAspectRatioPanel);
            PercentSetting = new VisualElement();

            TagPanel = new Image().AddClass("TagPanel").AddElements(PixelResizeTag, PercentResizeTag);

            this.AddElements(TagPanel).AddSpace(0, 10);
            SwitchTag(true);
        }

        public void SwitchTag(bool pixelTag)
        {
            PixelResizeTag.EnableClass(!pixelTag, "Tag_Disable");
            PercentResizeTag.EnableClass(pixelTag, "Tag_Disable");

            if (pixelTag)
            {
                PercentSetting.RemoveFromHierarchy();
                this.AddElements(PixelSetting);
            }
            else
            {
                PixelSetting.RemoveFromHierarchy();
                this.AddElements(PercentSetting);
            }
        }
        public void WidthChanged(ChangeEvent<string> evt)
        {
            evt.StopPropagation();
            OnWidthChaned?.Invoke(evt.newValue.EToInt());
        }
        public void HeightChanged(ChangeEvent<string> evt)
        {
            evt.StopPropagation();
            OnHeightChaned?.Invoke(evt.newValue.EToInt());
        }
    }
}
#endif