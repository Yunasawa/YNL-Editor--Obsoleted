#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editors.Windows.Utilities;
using YNL.Editors.UIElements.Styled;

namespace YNL.Editors.Windows.Texture.ImageInverter
{
    public class WTextureImageInverter_Visual : EVisual
    {
        private const string _styleSheet = "Style Sheets/Windows/W - Utilities/Texture - Image Inverter/WTextureImageInverter";

        #region ▶ Editor Properties
        private float _tagPanelWidth = 200;
        private EMinMax _propertyPanelWidth = new EMinMax(100, 200);

        public float ImageWidth => _sizeSlider.Slider.value.ERemap(new(0, 10), _propertyPanelWidth);
        #endregion
        #region ▶ Visual Elements
        private WTextureImageInverter_Main _main;

        private EWindowTitle _windowTitlePanel;
        private EWindowTagPanel _tagPanel;
        private EInteractableImage _propertyPanel;

        private VisualElement _handlerWindow;
        private Image _mainWindow;

        private EImageDisplayer _displayer;

        private Button _clearButton;
        private ESlider _sizeSlider;

        public EInvertExecutePanel InvertExecutePanel;
        #endregion

        public WTextureImageInverter_Visual(EWindowTagPanel tagPanel, WTextureImageInverter_Main main)
        {
            SetWindowTitle
            (
                "Textures/Windows/Texture Center/Image Inverter/Image Inverter Icon 2",
                "Texture - Image Inverter",
                "Flip the script, invert your images!"
            );

            _main = main;
            _tagPanel = tagPanel;

            this.AddStyle(_styleSheet, EStyleSheet.Font).AddClass("Main");

            CreateElements();

            PanelMarginHandler();

            this.AddElements(_handlerWindow, _windowTitlePanel, _propertyPanel);
        }

        private void CreateElements()
        {
            _windowTitlePanel = new(_windowIcon.ELoadAsset<Texture2D>(), _windowTitle, _windowSubtitle);
            _windowTitlePanel.AddClass("WindowTitle");

            _clearButton = new Button(ClearBoxes).AddClass("ClearButton");
            _clearButton.AddLabel("Clear all images", "ClearLabel");

            _sizeSlider = new ESlider(_propertyPanelWidth).AddClass("SizeSlider");
            _sizeSlider.Slider.value = 0;
            _sizeSlider.OnValueChanged += ResizeImageBox;

            InvertExecutePanel = new EInvertExecutePanel();
            InvertExecutePanel.Execute.clicked += InvertImages;

            _propertyPanel = new EInteractableImage().AddElements(_sizeSlider, _clearButton, InvertExecutePanel);
            _propertyPanel.AddClass("PropertyPanel");

            _displayer = new EImageDisplayer().AddClass("MainWindow");
            
            _mainWindow = new Image().AddClass("MainWindow").AddElements(_displayer);
            _handlerWindow = new VisualElement().AddClass("HandlerWindow").AddElements(_mainWindow);
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

        private void ClearBoxes()
        {
            _displayer.ClearItems();
            _main.Handler.Textures.Clear();
        }

        public void ResizeImageBox(float size)
        {
            foreach (var box in _displayer.Grid.Children().ToArray())
            {
                box.SetSize(size * 2.5f, size * 1.5f);
            }
        }

        public void GenerateImages(Texture2D[] textures, float width)
        {
            _displayer.GenerateImages(textures, width);
        }

        public void InvertImages()
        {
            foreach (EImageBox box in _displayer.Grid.Children().ToArray())
            {
                _main.Handler.InvertImage(box.AssignedImage, InvertExecutePanel.IsReplaceOldImage, box);
            }

            AssetDatabase.Refresh();
        }
    }
}
#endif