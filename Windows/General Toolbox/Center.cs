#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using YNL.Extensions.Methods;
using YNL.Editors.UIElements.Styled;
using YNL.Editors.Windows.TextureImageResizer;
using YNL.Editors.Windows.AnimationObjectRenamer;
using YNL.Editors.Windows.TextureImageInverter;
using YNL.Editors.Extensions;

namespace YNL.Editors.Windows
{
    public class Center : EditorWindow
    {
        #region ▶ Editor Asset Fields/Properties
        private const string _windowIconPath = "Textures/Windows/Utilities Center/Editor Window Icon";
        #endregion

        #region ▶ Visual Elements
        public StyledWindowTagPanel WindowTagPanel;

        public TextureImageResizer.Main ImageResizerWindow;
        public TextureImageInverter.Main ImageInverterWindow;
        public AnimationObjectRenamer.Main ObjectRenamerWindow;
        #endregion

        #region ▶ General Fields/Properties
        private static CenterData _staticCenterData;
        private CenterData _centerData
        {
            get
            {
                if (_staticCenterData.IsNull()) _staticCenterData = "Editor Toolbox Data".LoadResource<CenterData>();
                return _staticCenterData;
            }
        }

        private float _tagPanelWidth = 200;

        private IMain _selectedWindow;
        #endregion


        [MenuItem("🔗 YのL/🔗 Windows/🔗 General Toolbox")]
        public static void ShowWindow()
        {
            Center window = GetWindow<Center>("General Toolbox");
            Texture2D texture = Resources.Load<Texture2D>(_windowIconPath);

            window.titleContent.image = texture;
            window.maxSize = new Vector2(800, 500);
            window.minSize = window.maxSize;
        }

        #region ▶ Editor Messages
        private void OnSelectionChange()
        {
            if (!_selectedWindow.IsNull()) _selectedWindow.OnSelectionChange();
        }

        public void CreateGUI()
        {
            Texture2D windowIcon = Resources.Load<Texture2D>("Textures/Windows/Utilities Center/Editor Icon");
            
            Texture2D textureImageResizerIcon = Resources.Load<Texture2D>("Textures/Windows/Texture Center/Image Resizer/Image Resizer Icon");
            Texture2D textureImageInverterIcon = Resources.Load<Texture2D>("Textures/Windows/Texture Center/Image Inverter/Image Inverter Icon 2");
            Texture2D animationObjectRenamerIcon = Resources.Load<Texture2D>("Textures/Windows/Animation Center/Cracking Bone");
            
            Texture2D waitIcon = Resources.Load<Texture2D>("Textures/Icons/Time1");

            WindowTagPanel = new(windowIcon, "General Toolbox", "Center", _tagPanelWidth, new StyledWindowTag[]
            {
            new StyledWindowTag(textureImageResizerIcon, "Image Resizer", "Texture", Color.white, _tagPanelWidth - 25, () => SwitchWindow(WindowType.TextureImageResizer)),
            new StyledWindowTag(textureImageInverterIcon, "Image Inverter", "Texture", Color.white, _tagPanelWidth - 25, () => SwitchWindow(WindowType.TextureImageInverter)),
            new StyledWindowTag(animationObjectRenamerIcon, "Object Renamer", "Animation", Color.white, _tagPanelWidth - 25, () => SwitchWindow(WindowType.AnimationObjectRenamer)),
            new StyledWindowTag(waitIcon, "Coming Soon", "", Color.white, _tagPanelWidth - 25, () => SwitchWindow(WindowType.C))
            });

            ImageResizerWindow = new TextureImageResizer.Main(this, WindowTagPanel);
            ObjectRenamerWindow = new AnimationObjectRenamer.Main(this, WindowTagPanel);
            ImageInverterWindow = new TextureImageInverter.Main(this, WindowTagPanel);

            SwitchWindow(_centerData.CurrentWindow);
        }

        public void OnGUI()
        {
            if (!_selectedWindow.IsNull()) _selectedWindow.OnGUI();
        }
        #endregion

        private void SwitchWindow(WindowType windowTag)
        {
            rootVisualElement.RemoveAllElements();
            switch (windowTag)
            {
                case WindowType.TextureImageResizer:
                    SwitchWindow(ImageResizerWindow);
                    rootVisualElement.Add(ImageResizerWindow.Visual);
                    break;
                case WindowType.TextureImageInverter:
                    SwitchWindow(ImageInverterWindow);
                    rootVisualElement.Add(ImageInverterWindow.Visual);
                    break;
                case WindowType.AnimationObjectRenamer:
                    SwitchWindow(ObjectRenamerWindow);
                    rootVisualElement.Add(ObjectRenamerWindow.Visual);
                    break;
            }
            rootVisualElement.Add(WindowTagPanel);
            _centerData.CurrentWindow = windowTag;
            EditorUtility.SetDirty(_centerData);
            AssetDatabase.SaveAssets();
        }
        
        private void SwitchWindow(IMain window)
        {
            if (!_selectedWindow.IsNull()) WindowTagPanel.Tutorial.clicked -= _selectedWindow.OpenInstruction;
            _selectedWindow = window;
            WindowTagPanel.Tutorial.clicked += _selectedWindow.OpenInstruction;
        }
    }

    public enum WindowType
    {
        TextureImageResizer, TextureImageInverter, AnimationObjectRenamer, C, D, E, F
    }
}
#endif
