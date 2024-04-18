#if UNITY_EDITOR
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editor.Extension;
using YNL.Editor.UIElement;
using YNL.Editor.Utility;

namespace YNL.Editor.Window.Texture.ImageResizer
{
    public class WTextureImageResizer_Visual : VisualElement
    {
        #region ▶ Editor Constants
        private const string _windowIcon = "Textures/Windows/Texture Center/Image Resizer/Image Resizer Icon";
        private const string _windowTitle = "Texture - Image Resizer";
        private const string _windowSubtitle = "Resize your images quickly and flexibly";
        #endregion
        #region ▶ Editor Properties
        private float _tagPanelWidth = 200;
        private EMinMax _propertyPanelWidth = new EMinMax(100, 200);
        public float ImageWidth => _slider.Slider.value.Map(new(0, 10), _propertyPanelWidth);
        #endregion
        #region ▶ Style Sheets
        private const string _uss_Main = "Style Sheets/Windows/W - Utilities/Texture - Image Resizer/WTextureImageResizer";
        #endregion
        #region ▶ Visual Elements
        private WTextureImageResizer_Main _main;

        private EWindowTitle _windowTitlePanel;
        private EWindowTagPanel _tagPanel;
        private EInteractableImage _propertyPanel;

        private VisualElement _handlerWindow;
        private Image _mainWindow;

        private EImageDisplayer _displayer;

        private Button _clearButton;
        private ESlider _slider;
        public EResizeSettingPanel ResizeSettingPanel;
        public EResizeExecutePanel ResizeExecutePanel;
        #endregion
        #region ▶ Visual Classes
        private const string _class_root = "main-root";
        private const string _class_titlePanel = "title-panel";
        private const string _class_propertyPanel = "property-panel";
        #endregion

        public WTextureImageResizer_Visual(EWindowTagPanel tagPanel, WTextureImageResizer_Main main)
        {
            _tagPanel = tagPanel;
            _main = main;

            CreateElements();
            AddClasses();

            ResizingSetter();
            ResizingExecuter();

            PanelMarginHandler();

            this.AddElements(_handlerWindow, _windowTitlePanel, _propertyPanel);
        }

        private void CreateElements()
        {
            _windowTitlePanel = new(_windowIcon.LoadAsset<Texture2D>(), _windowTitle, _windowSubtitle);

            _slider = new ESlider(_propertyPanelWidth).AddClass("slider");
            _slider.Slider.value = 5;
            _slider.OnValueChanged += ResizeImageBox;

            _clearButton = new Button(ClearItems).AddClass("clear-button");
            _clearButton.AddLabel("Clear all images", "clear-label");

            ResizeSettingPanel = new EResizeSettingPanel();
            ResizeExecutePanel = new EResizeExecutePanel();
            _propertyPanel = new EInteractableImage().AddElements(_slider, _clearButton, ResizeSettingPanel, ResizeExecutePanel);

            _displayer = new EImageDisplayer().AddClass("main-window");
            _mainWindow = new Image().AddClass("main-window").AddElements(_displayer);
            _handlerWindow = new VisualElement().AddClass("handler-window").AddElements(_mainWindow);
        }
        private void AddClasses()
        {
            this.AddStyle(_uss_Main, EAddress.USSFont).AddClass(_class_root);

            _windowTitlePanel.AddClass(_class_titlePanel);
            _propertyPanel.AddClass(_class_propertyPanel);
        }

        public void GenerateImages(Texture2D[] textures, float width)
        {
            Vector2 newSize = new Vector2(ResizeSettingPanel.Width.value.ToInt(), ResizeSettingPanel.Height.value.ToInt());
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
                _displayer.SetAllNewAspectedSize(ResizeSettingPanel.Width.value.ToInt(), true);
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