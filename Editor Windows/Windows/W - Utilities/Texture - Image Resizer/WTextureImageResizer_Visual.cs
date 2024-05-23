#if UNITY_EDITOR
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editors.Windows.Utilities;
using YNL.Editors.UIElements.Styled;

namespace YNL.Editors.Windows.Texture.ImageResizer
{
    public class WTextureImageResizer_Visual : EVisual
    {
        private const string _styleSheet = "Style Sheets/Windows/W - Utilities/Texture - Image Resizer/WTextureImageResizer";

        #region ▶ Editor Properties
        private float _tagPanelWidth = 200;
        private EMinMax _propertyPanelWidth = new EMinMax(100, 200);
        public float ImageWidth => _sizeSlider.Slider.value.ERemap(new(0, 10), _propertyPanelWidth);
        #endregion
        #region ▶ Visual Elements
        private WTextureImageResizer_Main _main;

        private StyledWindowTitle _windowTitlePanel;
        private StyledWindowTagPanel _tagPanel;
        private StyledInteractableImage _propertyPanel;

        private VisualElement _handlerWindow;
        private Image _mainWindow;

        private EImageDisplayer _displayer;

        private Button _clearButton;
        private StyledSlider _sizeSlider;
        public EResizeSettingPanel ResizeSettingPanel;
        public EResizeExecutePanel ResizeExecutePanel;
        #endregion
        #region ▶ Visual Classes
        private const string _class_root = "main-root";
        private const string _class_titlePanel = "title-panel";
        private const string _class_propertyPanel = "property-panel";
        #endregion

        public WTextureImageResizer_Visual(StyledWindowTagPanel tagPanel, WTextureImageResizer_Main main)
        {
            SetWindowTitle
            (
                "Textures/Windows/Texture Center/Image Resizer/Image Resizer Icon",
                "Texture - Image Resizer",
                "Resize with ease, preserve the quality!"
            );

            _tagPanel = tagPanel;
            _main = main;

            this.AddStyle(_styleSheet, EStyleSheet.Font).AddClass(_class_root);

            CreateElements();

            ResizingSetter();
            ResizingExecuter();

            PanelMarginHandler();

            this.AddElements(_handlerWindow, _windowTitlePanel, _propertyPanel);
        }

        private void CreateElements()
        {
            _windowTitlePanel = new(_windowIcon.ELoadAsset<Texture2D>(), _windowTitle, _windowSubtitle);
            _windowTitlePanel.AddClass(_class_titlePanel);

            _sizeSlider = new StyledSlider(_propertyPanelWidth).AddClass("slider");
            _sizeSlider.Slider.value = 5;
            _sizeSlider.OnValueChanged += ResizeImageBox;

            _clearButton = new Button(ClearItems).AddClass("clear-button");
            _clearButton.AddLabel("Clear all images", "clear-label");

            ResizeSettingPanel = new EResizeSettingPanel();
            ResizeExecutePanel = new EResizeExecutePanel();
            
            _propertyPanel = new StyledInteractableImage().AddElements(_sizeSlider, _clearButton, ResizeSettingPanel, ResizeExecutePanel);
            _propertyPanel.AddClass(_class_propertyPanel);

            _displayer = new EImageDisplayer().AddClass("main-window");
            _mainWindow = new Image().AddClass("main-window").AddElements(_displayer);
            _handlerWindow = new VisualElement().AddClass("handler-window").AddElements(_mainWindow);
        }

        public void GenerateImages(Texture2D[] textures, float width)
        {
            Vector2 newSize = new Vector2(ResizeSettingPanel.Width.value.EToInt(), ResizeSettingPanel.Height.value.EToInt());
            _displayer.GenerateImages(textures, width, newSize, ResizeSettingPanel.KeepAspectRatioSwitch.Enable);
        }
        public void ClearItems()
        {
            _displayer.ClearItems();
            _main.Handler.Textures.Clear();
        }
        public void ResizeImageBox(float size)
        {
            foreach (var box in _displayer.Grid.Children().ToArray())
            {
                box.SetSize(size, size * 1.5f);
            }
        }
        public void ResizingSetter()
        {
            ResizeSettingPanel.OnWidthChaned += (width) =>
            {
                if (ResizeSettingPanel.KeepAspectRatioSwitch.Enable)
                {
                    _displayer.SetAllNewAspectedSize(width, true);
                    //ResizeSettingPanel.Height.SetText("aaa");
                }
                else _displayer.SetAllNewSize(width, true);
            };
            ResizeSettingPanel.OnHeightChaned += (height) =>
            {
                if (ResizeSettingPanel.KeepAspectRatioSwitch.Enable)
                {
                    if (ResizeSettingPanel.Height.value == "auto") return;
                    _displayer.SetAllNewAspectedSize(height, false);
                    //ResizeSettingPanel.Width.SetText("auto");
                }
                else _displayer.SetAllNewSize(height, false);
            };
            ResizeSettingPanel.KeepAspectRatioSwitch.OnSwitch += (enable) =>
            {
                if (!enable) return;
                _displayer.SetAllNewAspectedSize(ResizeSettingPanel.Width.value.EToInt(), true);
                ResizeSettingPanel.Height.SetText("auto");
            };
        }
        public void ResizingExecuter()
        {
            ResizeExecutePanel.Execute.clicked += () =>
            {
                foreach (EImageBox box in _displayer.Grid.Children().ToArray())
                {
                    _main.Handler.ResizeImage(box.AssignedImage, box.NewAssignedSize, ResizeExecutePanel.IsReplaceOldImage);
                    box.RefreshOriginSizeLabel();
                }
            };
        }

        private void PanelMarginHandler()
        {
            _tagPanel.OnPointerEnter += () =>
            {
                _propertyPanel.SetMarginLeft(_tagPanelWidth).SetWidth(100);
                _windowTitlePanel.Panel.SetMarginLeft(_tagPanelWidth - 150);
                _mainWindow.SetMarginLeft(_tagPanelWidth + 102);
            };
            _tagPanel.OnPointerExit += () =>
            {
                _propertyPanel.SetMarginLeft(50).SetWidth(200);
                _windowTitlePanel.Panel.SetMarginLeft(0);
                _mainWindow.SetMarginLeft(252);
            };
        }
    }
}
#endif