#if UNITY_EDITOR && YNL_UTILITIES
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Extensions.Methods;
using System.Collections.Generic;
using System.Linq;
using System;
using YNL.Editors.UIElements.Styled;
using YNL.Extensions.Addons;
using YNL.Editors.Extensions;
using UnityEditor;
using YNL.Utilities.Extensions;

namespace YNL.Editors.Windows.AnimationObjectRenamer
{
    public class Visual : EVisual
    {
        private const string _styleSheet = "Style Sheets/Windows/W - Utilities/Animation - Object Renamer/WAnimationObjectRenamer";
        
        #region ▶ Visual Elements
        private StyledWindowTitle _windowTitlePanel;
        private StyledWindowTagPanel _tagPanel;
        private StyledInteractableImage _propertyPanel;

        private Button _animatorPanel;
        private static ScrollView _clipsScroll;

        private Button _automaticPanel;

        private Label _referencedAnimatorTitle;
        private Label _referencedClipsTitle;
        public static StyledComponentField<Animator> ReferencedAnimator;

        private VisualElement _handlerWindow;

        private Image _animatorWindow;

        private EInputNamePanel _inputNamePanel;
        private static ERootNamePanel _rootNamePanel;

        private Image _automaticWindow;
        private static EAutomaticLogPanel _automaticLogPanel;

        private Button _modePanel;
        private Button _autoButton;
        private Image _autoIcon;
        private Label _autoLabel;
        private Button _manualButton;
        private Image _manualIcon;
        private Label _manualLabel;

        private StyledInteractableButton _enableButton;
        private Image _enableIcon;
        private Label _enableLabel;
        #endregion
        #region ▶ Style Classes
        private const string _class_root = "RootWindow";

        private const string _class_propertyPanel = "PropertyPanel";
        private const string _class_clipsScroll = "ClipsScroll";

        private const string _class_animatorField = "AnimatorField";

        private const string _class_handlerWindow = "HandlerWindow";
        private const string _class_titlePanel = "TitlePanel";
        #endregion
        #region ▶ General Fields/Properties
        private bool _createdAllElements = false;
        private float _tagPanelWidth = 200;
        private MRange _propertyPanelWidth = new MRange(100, 300);

        private Main _main;

        #endregion

        public Visual(StyledWindowTagPanel tagPanel, Main main)
        {
            SetWindowTitle
            (
                "Textures/Windows/Animation Center/Cracking Bone",
                "Animation - Object Renamer",
                "Easily change or swap your animation objects' name"
            );

            _tagPanel = tagPanel;
            _main = main;

            this.AddStyle(_styleSheet, ESheet.Font);

            CreateElements();
            AddClasses();

            TagPanelHandlers();
            PropertyPanelHandler();
            HandlerWindowCreator();

            this.AddElements(_handlerWindow, _windowTitlePanel, _propertyPanel);

            UpdateMode(false);
            UpdateAutomatic();

            RefreshLogPanel();

            _createdAllElements = true;
        }

        public void OnGUI()
        {
            if (!_createdAllElements) return;
        }

        #region ▶ Editor Initializing
        private void CreateElements()
        {
            #region Mode Button Fields
            _autoIcon = new Image().SetName("ModeIcon").SetBackgroundImage("Textures/Windows/Animation Center/Auto").SetMarginLeft(10).SetLeft(-5);
            _autoLabel = new Label().SetName("ModeLabel").SetText("Automatic");
            _autoButton = new Button().SetName("ModeButton").AddElements(_autoIcon, _autoLabel);
            _autoButton.clicked += () => _main.Handler.ChangeMode(true);

            _manualIcon = new Image().SetName("ModeIcon").SetBackgroundImage("Textures/Windows/Animation Center/Manual").SetMarginLeft(20).SetLeft(-15);
            _manualLabel = new Label().SetName("ModeLabel").SetText("Manual");
            _manualButton = new Button().SetName("ModeButton").AddElements(_manualIcon, _manualLabel);
            _manualButton.clicked += () => _main.Handler.ChangeMode(false);

            UpdateModePanel();

            _autoLabel.SetColor("#00000000");
            _manualLabel.SetColor("#00000000");

            _modePanel = new Button().AddClass("ModePanel").AddElements(_autoButton, _manualButton);
            #endregion

            #region Automatic Panel
            _enableIcon = new Image().AddClass("AutomaticEnableIcon");

            _enableLabel = new Label().AddClass("AutomaticEnableLabel").SetColor("#00000000");

            _enableButton = new StyledInteractableButton().AddClass("AutomaticEnableButton").AddElements(_enableLabel, _enableIcon);
            _enableButton.clicked += _main.Handler.SwitchAutomaticMode;

            _automaticPanel = new Button().AddClass("AutomaticPanel").AddElements(_enableButton);
            #endregion

            #region Animator Panel
            _referencedClipsTitle = new Label("Animation Clips:").AddClass("ClipsTitle");
            _clipsScroll = new ScrollView().SetWidth(100, true).SetHeight(100, true);

            _referencedAnimatorTitle = new Label("Referenced Animator:").AddClass("AnimatorTitle");
            ReferencedAnimator = new StyledComponentField<Animator>(Handler.ReferencedAnimator);
            ReferencedAnimator.Background.OnDragPerform += (obj) => PresentAllPaths();

            _animatorPanel = new Button()
                .AddSpace(0, 10).AddClass("AnimatorPanel")
                .AddElements(_referencedAnimatorTitle, ReferencedAnimator, _referencedClipsTitle, _clipsScroll);
            #endregion

            _propertyPanel = new StyledInteractableImage().AddElements(_modePanel);

            _windowTitlePanel = new(_windowIcon.LoadResource<Texture2D>(), _windowTitle, _windowSubtitle);
        }
        private void AddClasses()
        {
            this.AddClass(_class_root);

            _propertyPanel.AddClass(_class_propertyPanel);
            _clipsScroll.AddClass(_class_clipsScroll);

            ReferencedAnimator.AddClass(_class_animatorField);

            _windowTitlePanel.AddClass(_class_titlePanel);
        }
        #endregion
        #region ▶ Specific Elements Handlers
        private void TagPanelHandlers()
        {
            _tagPanel.OnPointerEnter += () =>
            {
                _propertyPanel.SetMarginLeft(_tagPanelWidth);
                _windowTitlePanel.Panel.SetMarginLeft(_tagPanelWidth - 50);
                _animatorWindow.SetMarginLeft(_tagPanelWidth + 102);
                _automaticWindow.SetMarginLeft(_tagPanelWidth + 102);
            };
            _tagPanel.OnPointerExit += () =>
            {
                _propertyPanel.SetMarginLeft(50);
                _windowTitlePanel.Panel.SetMarginLeft(0);
                _animatorWindow.SetMarginLeft(152);
                _automaticWindow.SetMarginLeft(152);
            };
        }
        private void PropertyPanelHandler()
        {
            _propertyPanel.OnPointerEnter = () =>
            {
                _propertyPanel.SetWidth(_propertyPanelWidth.Max);
                _windowTitlePanel.Panel.SetMarginLeft(_propertyPanelWidth.Max - 100);
                _animatorWindow.SetMarginLeft(_tagPanelWidth + 152);
                _automaticWindow.SetMarginLeft(_tagPanelWidth + 152);

                _autoIcon.SetLeft(0);
                _manualIcon.SetLeft(0);

                _autoLabel.SetColor(Variable.IsAutomaticPanel ? "#ffffff" : "#3d3d3d");
                _manualLabel.SetColor(!Variable.IsAutomaticPanel ? "#ffffff" : "#3d3d3d");

                _enableLabel.SetColor(Variable.IsAutomaticOn ? "#52ffa3" : "#ff5252");
            };
            _propertyPanel.OnPointerExit = () =>
            {
                _propertyPanel.SetWidth(_propertyPanelWidth.Min);
                _windowTitlePanel.Panel.SetMarginLeft(_propertyPanelWidth.Min - 100);
                _animatorWindow.SetMarginLeft(_propertyPanelWidth.Min + 52);
                _automaticWindow.SetMarginLeft(_propertyPanelWidth.Min + 52);

                _autoIcon.SetLeft(-5);
                _manualIcon.SetLeft(-15);

                _autoLabel.SetColor("#00000000");
                _manualLabel.SetColor("#00000000");

                _enableLabel.SetColor("#00000000");
            };
        }

        private void HandlerWindowCreator()
        {
            #region Animator Window
            _inputNamePanel = new EInputNamePanel();
            _inputNamePanel.SwapButton.OnPointerDown += () => ReplaceClipPathItem(_inputNamePanel.OriginField.text, _inputNamePanel.NewField.text, out bool isSucceeded);

            _rootNamePanel = new ERootNamePanel();

            _animatorWindow = new Image().AddClass("MainWindow");
            _animatorWindow.AddElements(_inputNamePanel, _rootNamePanel.SetMarginTop(0));
            #endregion

            #region Automatic WIndow
            _automaticLogPanel = new EAutomaticLogPanel();

            _automaticWindow = new Image().AddClass("MainWindow");
            _automaticWindow.AddElements(_automaticLogPanel);
            #endregion

            _handlerWindow = new VisualElement().AddClass(_class_handlerWindow);
        }
        #endregion
        #region ▶ Editor Functions Handlers
        public static void PresentAllClips(Dictionary<AnimationClip, Color> clips)
        {
            _clipsScroll.RemoveAllElements();

            foreach (var clip in clips)
            {
                _clipsScroll.AddElements(new EAnimationClipField(clip));
            }
        }
        public static void PresentAllPaths()
        {
            Handler.GetReferencedAnimator();
            Handler.FillModel();
            _rootNamePanel.ClearAllClipItem();
            if (Handler.Paths != null && !Handler.AnimationClips.IsEmpty())
            {
                if (Handler.PathsKeys.Count > 0)
                {
                    foreach (string path in Handler.PathsKeys)
                    {
                        CreateClipPathItem(path);
                    }
                }
                else
                {
                    _rootNamePanel.AddBoard("No animation clip found!");
                }
            }
            else
            {
                _rootNamePanel.AddBoard("No animation clip found!");
            }
        }
        public static void ReplaceClipPathItem(string oldPath, string newPath, out bool isSucceeded, bool isAuto = false)
        {
            isSucceeded = true;

            if (!Handler.AnimationClips.IsEmpty() && Handler.PathsKeys.Count > 0)
            {
                List<string> paths = new();

                foreach (var path in Handler.PathsKeys) paths.Add((string)path);

                if (paths.Contains(oldPath) && paths.Contains(newPath))
                {
                    ReplaceRoot();

                    if (!Variable.IsAutomaticPanel) MDebug.Custom("Swap", $"{oldPath} ▶ {newPath}", CColor.Macaroon.ToHex());
                }
                else
                {
                    Handler.ReplaceRoot(oldPath, newPath, () => ChangeVisuals(oldPath, newPath));

                    if (!Variable.IsAutomaticPanel && !isAuto) MDebug.Custom("Rename", $"{oldPath} ▶ {newPath}", CColor.Flamingo.ToHex());
                }
            }

            void ReplaceRoot()
            {
                Handler.ReplaceRoot(oldPath, "Temporary Root", () => ChangeVisuals(oldPath, "Temporary Root"));
                Handler.ReplaceRoot(newPath, oldPath, () => ChangeVisuals(newPath, oldPath));
                Handler.ReplaceRoot("Temporary Root", newPath, () => ChangeVisuals("Temporary Root", newPath));
            }

            void ChangeVisuals(string originalRoot, string newRoot)
            {
                EClipNameField clipNameField = _rootNamePanel.ClipPanel.Query<EClipNameField>().ToList().FirstOrDefault(i => i.Name.text == originalRoot);

                if (!clipNameField.IsNull())
                {
                    clipNameField.Name.SetText(newRoot);

                    GameObject returnedObject = Handler.FindObjectInRoot(newRoot);
                    clipNameField.Object.DragPerformOnField(returnedObject);

                    clipNameField.UpdateArrowColor();
                }
            }
        }
        public static void CreateClipPathItem(string path)
        {
            string newPath = path;
            GameObject gameObject = Handler.FindObjectInRoot(path);

            string pathOverride = path;
            string currentPath = path;

            List<Color> referencedColor = new();

            if (Handler.TempPathOverrides.ContainsKey(path)) pathOverride = Handler.TempPathOverrides[path];
            if (pathOverride != path) Handler.TempPathOverrides[path] = pathOverride;

            if (Handler.PathColors.ContainsKey(path))
            {
                foreach (var clip in Handler.AnimationClips)
                {
                    if (Handler.ClipColors.ContainsKey(clip))
                    {
                        if (Handler.PathColors[path].Contains(clip)) referencedColor.Add(Handler.ClipColors[clip]);
                        else referencedColor.Add(Color.clear);
                    }
                    //else MDebug.Caution("Do nothing");
                }
            }

            Color arrowColor = "#BF4040".ToColor();
            if (!gameObject.IsNull()) arrowColor = "#40BF8F".ToColor();

            EClipNameField clipNameField = new(pathOverride, gameObject, referencedColor.ToArray(), arrowColor, null);

            clipNameField.Object.Background.OnDragPerform += (newObject) =>
            {
                ClipPathObjectChanged(clipNameField, gameObject, newObject, ref currentPath);
            };

            clipNameField.ChangeButton.OnPointerDown += () =>
            {
                ClipPathRootChanged(clipNameField, ref currentPath, ref newPath, clipNameField.Name.text, null);
            };

            clipNameField.UndoButton.OnPointerDown += () =>
            {
                ClipPathRootChanged(clipNameField, ref currentPath, ref newPath, clipNameField.LastRoot, () =>
                {
                    clipNameField.Name.SetText(currentPath);
                });
            };

            _rootNamePanel.AddClipItem(clipNameField);
        }
        public static void ClipPathObjectChanged(EClipNameField clipNameField, GameObject gameObject, GameObject newObject, ref string currentPath)
        {
            gameObject = Handler.FindObjectInRoot(currentPath);

            try
            {
                if (gameObject != newObject)
                {
                    //MDebug.Caution($"Start: {gameObject.CheckNull("gameObject")?.name} - {newObject.CheckNull("newObject")?.name} | {clipNameField.CheckNull("clipNameField")?.Name.text} - {currentPath.CheckNull("currentPath")}");

                    currentPath = Handler.ChildPath(newObject);
                    Handler.UpdatePath(clipNameField.Name.text, currentPath);
                    clipNameField.Name.SetText(currentPath);
                    gameObject = newObject;
                    Handler.FillModel();

                    //MDebug.Caution($"End: {gameObject.CheckNull("gameObject")?.name} - {newObject.CheckNull("newObject")?.name} | {clipNameField.CheckNull("clipNameField")?.Name.text} - {currentPath.CheckNull("currentPath")}");

                }
            }
            catch (UnityException)
            {
                MDebug.Caution($"<color=#c7ff96><b>{currentPath}</b></color> already exits in animation!");
                GameObject returnedObject = Handler.FindObjectInRoot(clipNameField.Name.text);
                clipNameField.Object.DragPerformOnField(returnedObject);
            }
            clipNameField.UpdateArrowColor();
        }
        public static void ClipPathRootChanged(EClipNameField clipNameField, ref string currentPath, ref string newPath, string setNewPath, Action additionAction)
        {
            newPath = setNewPath;
            Handler.TempPathOverrides.Remove(currentPath);

            try
            {
                if (newPath != currentPath)
                {
                    clipNameField.LastRoot = currentPath;

                    Handler.UpdatePath(currentPath, newPath);
                    currentPath = newPath;

                    GameObject getObject = Handler.FindObjectInRoot(currentPath);

                    clipNameField.Object.DragPerformOnField(getObject);
                    clipNameField.UpdateArrowColor();

                    additionAction?.Invoke();

                    Handler.FillModel();
                }
            }
            catch (UnityException)
            {
                MDebug.Caution($"<color=#c7ff96><b>{currentPath}</b></color> already exits in animation!");
                clipNameField.Name.SetText(currentPath);
            }
        }

        public void UpdateMode(bool updateModePanel)
        {
            if (updateModePanel) UpdateModePanel();

            if (Variable.IsAutomaticPanel)
            {
                _animatorPanel.RemoveFromHierarchy();
                _propertyPanel.AddElements(_automaticPanel);

                _animatorWindow.RemoveFromHierarchy();
                _handlerWindow.AddElements(_automaticWindow);
            }
            else
            {
                _automaticPanel.RemoveFromHierarchy();
                _propertyPanel.AddElements(_animatorPanel);

                _automaticWindow.RemoveFromHierarchy();
                _handlerWindow.AddElements(_animatorWindow);
            }
        }
        public void UpdateModePanel()
        {
            _autoLabel.SetColor(Variable.IsAutomaticPanel ? "#ffffff" : "#3d3d3d");
            _manualLabel.SetColor(!Variable.IsAutomaticPanel ? "#ffffff" : "#3d3d3d");

            _autoIcon.SetBackgroundImageTintColor(Variable.IsAutomaticPanel ? "#ffffff" : "#3d3d3d");
            _manualIcon.SetBackgroundImageTintColor(!Variable.IsAutomaticPanel ? "#ffffff" : "#3d3d3d");

            _autoButton.SetBackgroundColor(Variable.IsAutomaticPanel ? "#2d2d2d" : "#1f1f1f");
            _manualButton.SetBackgroundColor(!Variable.IsAutomaticPanel ? "#2d2d2d" : "#1f1f1f");
        }

        public void UpdateAutomatic()
        {
            _enableIcon.SetBackgroundImageTintColor(Variable.IsAutomaticOn ? "#52ffa3" : "#ff5252");

            _enableLabel.SetColor(Variable.IsAutomaticOn ? "#52ffa3" : "#ff5252");
            _enableLabel.SetText(Variable.IsAutomaticOn ? "Automatic Mode: On" : "Automatic Mode: Off");

            //_enableButton.SetBorderColor(Variable.IsAutomaticOn ? "#2e8c5a" : "#ab3e3e");
            _enableButton.SetBorderColor(Variable.IsAutomaticOn ? "#52ffa3" : "#ff5252");
        }
        public static void ClearLogPanel()
        {
            Variable.AutomaticLogs.Clear();
            RefreshLogPanel();
            Variable.SaveData();
        }
        public static void RefreshLogPanel()
        {
            _automaticLogPanel.ClearLogs();

            foreach (var line in Variable.AutomaticLogs)
            {
                _automaticLogPanel.AddLogItem(new(line));
            }
        }
        public static void UpdateLogPanel()
        {
            EAutomaticLogLine line = new(Variable.AutomaticLogs[^1]);

            _automaticLogPanel.AddLogItem(line);
        }
        #endregion
    }
}
#endif