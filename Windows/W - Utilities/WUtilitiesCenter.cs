#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using YNL.Editor.Extensions;
using YNL.Editor.UIElement;
using YNL.Editor.Utilities;
using YNL.Editor.Window.Texture.ImageResizer;
using YNL.Editor.Window.Animation.ObjectRenamer;
using Unity.Plastic.Newtonsoft.Json.Bson;
using System;

namespace YNL.Editor.Window
{
    public class WUtilitiesCenter : EditorWindow
    {
        #region ▶ Editor Asset Fields/Properties
        private const string _windowIconPath = "Textures/Windows/Utilities Center/Editor Window Icon";
        #endregion

        #region ▶ Visual Elements
        public EWindowTagPanel WindowTagPanel;

        public WTextureImageResizer_Main ImageResizerWindow;
        public WAnimationObjectRenamer_Main ObjectRenamerWindow;
        #endregion

        #region ▶ General Fields/Properties
        private float _tagPanelWidth = 200;

        private IWindow _selectedWindow;
        #endregion


        [MenuItem("🔗 YのL/🔗 Windows/🔗 Editor Utilities Center")]
        public static void ShowWindow()
        {
            WUtilitiesCenter window = GetWindow<WUtilitiesCenter>("Editor Utilities Center");
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
            
            Texture2D textureImageResizerIcon = Resources.Load<Texture2D>("Textures/Windows/Texture Center/Image Resizer Icon");
            Texture2D animationObjectRenamerIcon = Resources.Load<Texture2D>("Textures/Windows/Animation Center/Cracking Bone");
            
            Texture2D waitIcon = Resources.Load<Texture2D>("Textures/Icons/Time1");

            WindowTagPanel = new(windowIcon, "Editor Utilities", "Center", _tagPanelWidth, new EWindowTag[]
            {
            new EWindowTag(textureImageResizerIcon, "Image Resizer", "Texture", Color.white, _tagPanelWidth - 25, () => SwitchWindow(WUtilitiesWindowType.TextureImageResizer)),
            new EWindowTag(animationObjectRenamerIcon, "Object Renamer", "Animation", Color.white, _tagPanelWidth - 25, () => SwitchWindow(WUtilitiesWindowType.AnimationObjectRenamer)),
            new EWindowTag(waitIcon, "Coming Soon", "", Color.white, _tagPanelWidth - 25, () => SwitchWindow(WUtilitiesWindowType.C))
            });

            ImageResizerWindow = new WTextureImageResizer_Main(this, WindowTagPanel);
            ObjectRenamerWindow = new WAnimationObjectRenamer_Main(this, WindowTagPanel);

            SwitchWindow(WUtilitiesWindowType.TextureImageResizer);
        }

        public void OnGUI()
        {
            if (!_selectedWindow.IsNull()) _selectedWindow.OnGUI();
        }
        #endregion

        private void SwitchWindow(WUtilitiesWindowType windowTag)
        {
            rootVisualElement.RemoveAllElements();
            switch (windowTag)
            {
                case WUtilitiesWindowType.TextureImageResizer:
                    SwitchWindow(ImageResizerWindow);
                    rootVisualElement.Add(ImageResizerWindow.Visual);
                    break;
                case WUtilitiesWindowType.AnimationObjectRenamer:
                    SwitchWindow(ObjectRenamerWindow);
                    rootVisualElement.Add(ObjectRenamerWindow.Visual);
                    break;
            }
            rootVisualElement.Add(WindowTagPanel);
        }
        
        private void SwitchWindow(IWindow window)
        {
            if (!_selectedWindow.IsNull()) WindowTagPanel.Tutorial.clicked -= _selectedWindow.OpenInstruction;
            _selectedWindow = window;
            WindowTagPanel.Tutorial.clicked += _selectedWindow.OpenInstruction;
        }
    }

    public enum WUtilitiesWindowType
    {
        TextureImageResizer, AnimationObjectRenamer, C, D, E, F
    }
}
#endif
