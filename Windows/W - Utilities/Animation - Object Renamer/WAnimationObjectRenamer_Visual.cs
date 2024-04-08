//Sourced: https://github.com/s-m-k/Unity-Animation-Hierarchy-Editor/tree/master
//Remastered: Yunasawa

#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editor.Utilities;
using System.Collections.Generic;
using System.Linq;
using System;
using YNL.Editor.Extensions;

public class WAnimationObjectRenamer_Visual : VisualElement
{
    #region ▶ Editor Constants
    private const string _windowPath = "Style Sheets/Windows";
    private const string _texturePath = "Textures/Windows/Animation Center/";

    private const string _crackingBonePNG = _texturePath + "Cracking Bone.png";

    private const string _windowTitle = "Animation - Object Renamer";
    private const string _windowSubtitle = "Quickly change or swap your animation objects' name";
    #endregion
    #region ▶ Style Sheets
    private const string _uss_Main = "Style Sheets/Windows/W - Utilities/Animation - Object Renamer/WAnimationObjectRenamer";
    #endregion
    #region ▶ Visual Elements
    private EWindowTitle _windowTitlePanel;
    private EWindowTagPanel _tagPanel;
    private EInteractableImage _propertyPanel;

    private Button _animatorPanel;
    private Button _clipsPanel;
    private ScrollView _clipsScroll;

    private Label _referencedAnimatorTitle;
    private Label _referencedClipsTitle;
    public EComponentField<Animator> ReferencedAnimator;

    private VisualElement _handlerWindow;
    private Image _mainWindow;

    private EInputNamePanel _inputNamePanel;
    private ERootNamePanel _rootNamePanel;
    #endregion
    #region ▶ Style Classes
    private const string _class_root = "RootWindow";

    private const string _class_propertyPanel = "PropertyPanel";
    private const string _class_animatorPanel = "AnimatorPanel";
    private const string _class_clipsScroll = "ClipsScroll";
    private const string _class_clipsPanel = "ClipsPanel";

    private const string _class_animatorField = "AnimatorField";

    private const string _class_handlerWindow = "HandlerWindow";
    private const string _class_titlePanel = "TitlePanel";
    #endregion
    #region ▶ General Fields/Properties
    private bool _createdAllElements = false;
    private float _tagPanelWidth = 200;
    private MinMax _propertyPanelWidth = new MinMax(100, 300);

    private WAnimationObjectRenamer_Main _main;

    #endregion

    public WAnimationObjectRenamer_Visual(EWindowTagPanel tagPanel, WAnimationObjectRenamer_Main main)
    {
        _tagPanel = tagPanel;
        _main = main;

        AddStyles();
        CreateElements();
        AddClasses();

        TagPanelHandlers();
        PropertyPanelHandler();
        HandlerWindowCreator();

        this.AddElements(_handlerWindow, _windowTitlePanel, _propertyPanel);

        _createdAllElements = true;
    }

    public void OnGUI()
    {
        if (!_createdAllElements) return;
    }

    #region ▶ Editor Initializing
    private void AddStyles()
    {
        this.AddStyle(_uss_Main, EAddress.USSFont);
    }
    private void CreateElements()
    {
        _referencedClipsTitle = new Label("Animation Clips:").AddClass("ClipsTitle");
        _clipsScroll = new ScrollView().SetWidth(100, true).SetHeight(100, true);

        _clipsPanel = new Button().AddClass(_class_clipsPanel).AddElements(_clipsScroll, _referencedClipsTitle);

        _referencedAnimatorTitle = new Label("Referenced Animator:").AddClass("AnimatorTitle");
        ReferencedAnimator = new EComponentField<Animator>(_main.Handler.ReferencedAnimator);
        ReferencedAnimator.Background.OnDragPerform += (obj) => PresentAllPaths();

        _animatorPanel = new Button().AddSpace(0, 10).AddElements(_referencedAnimatorTitle, ReferencedAnimator);

        _propertyPanel = new EInteractableImage().AddElements(_animatorPanel, _clipsPanel);

        _windowTitlePanel = new(_crackingBonePNG.LoadAsset<Texture2D>(), _windowTitle, _windowSubtitle);
    }
    private void AddClasses()
    {
        this.AddClass(_class_root);

        _propertyPanel.AddClass(_class_propertyPanel);
        _animatorPanel.AddClass(_class_animatorPanel);
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
            _mainWindow.SetMarginLeft(_tagPanelWidth + 102);
        };
        _tagPanel.OnPointerExit += () =>
        {
            _propertyPanel.SetMarginLeft(50);
            _windowTitlePanel.Panel.SetMarginLeft(0);
            _mainWindow.SetMarginLeft(152);
        };
    }
    private void PropertyPanelHandler()
    {
        _propertyPanel.OnPointerEnter = () =>
        {
            _propertyPanel.SetWidth(_propertyPanelWidth.Max);
            _windowTitlePanel.Panel.SetMarginLeft(_propertyPanelWidth.Max - 100);
            _mainWindow.SetMarginLeft(_tagPanelWidth + 152);
        };
        _propertyPanel.OnPointerExit = () =>
        {
            _propertyPanel.SetWidth(_propertyPanelWidth.Min);
            _windowTitlePanel.Panel.SetMarginLeft(_propertyPanelWidth.Min - 100);
            _mainWindow.SetMarginLeft(_propertyPanelWidth.Min + 52);
        };
    }

    private void HandlerWindowCreator()
    {
        _handlerWindow = new VisualElement().AddClass(_class_handlerWindow);
        _mainWindow = new Image().AddClass("MainWindow");

        _inputNamePanel = new EInputNamePanel();
        _inputNamePanel.SwapButton.OnPointerDown += ReplaceClipPathItem;

        _rootNamePanel = new ERootNamePanel();

        _mainWindow.AddElements(_inputNamePanel, _rootNamePanel.SetMarginTop(0));

        _handlerWindow.AddElements(_mainWindow);
    }
    #endregion
    #region ▶ Editor Functions Handlers
    public void PresentAllClips(Dictionary<AnimationClip, Color> clips)
    {
        _clipsScroll.RemoveAllElements();

        foreach (var clip in clips)
        {
            _clipsScroll.AddElements(new EAnimationClipField(clip));
        }
    }
    public void PresentAllPaths()
    {
        _main.Handler.GetReferencedAnimator();
        _main.Handler.FillModel();
        _rootNamePanel.ClearAllClipItem();
        if (_main.Handler.Paths != null && !_main.Handler.AnimationClips.IsEmpty())
        {
            if (_main.Handler.PathsKeys.Count > 0)
            {
                foreach (string path in _main.Handler.PathsKeys)
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
    public void ReplaceClipPathItem()
    {
        string _originalRoot = _inputNamePanel.OriginField.text;
        string _newRoot = _inputNamePanel.NewField.text;

        if (!_main.Handler.AnimationClips.IsEmpty() && _main.Handler.PathsKeys.Count > 0)
        {
            List<string> paths = new();

            foreach (var path in _main.Handler.PathsKeys) paths.Add((string)path);

            if (paths.Contains(_originalRoot) && paths.Contains(_newRoot))
            {
                _main.Handler.ReplaceRoot(_originalRoot, "Temporary Root", () => ChangeVisuals(_originalRoot, "Temporary Root"));
                _main.Handler.ReplaceRoot(_newRoot, _originalRoot, () => ChangeVisuals(_newRoot, _originalRoot));
                _main.Handler.ReplaceRoot("Temporary Root", _newRoot, () => ChangeVisuals("Temporary Root", _newRoot));

                MDebug.Custom("Swap", $"{_originalRoot} ▶ {_newRoot}", CColor.Macaroon.ToHex());
            }
            else
            {
                _main.Handler.ReplaceRoot(_originalRoot, _newRoot, () => ChangeVisuals(_originalRoot, _newRoot));

                MDebug.Custom("Rename", $"{_originalRoot} ▶ {_newRoot}", CColor.Flamingo.ToHex());
            }
        }

        void ChangeVisuals(string originalRoot, string newRoot)
        {
            EClipNameField clipNameField = _rootNamePanel.ClipPanel.Query<EClipNameField>().ToList().FirstOrDefault(i => i.Name.text == originalRoot);

            if (!clipNameField.IsNull())
            {
                clipNameField.Name.SetText(newRoot);

                GameObject returnedObject = _main.Handler.FindObjectInRoot(newRoot);
                clipNameField.Object.DragPerformOnField(returnedObject);

                clipNameField.UpdateArrowColor();
            }
        }
    }
    public void CreateClipPathItem(string path)
    {
        string newPath = path;
        GameObject gameObject = _main.Handler.FindObjectInRoot(path);

        string pathOverride = path;
        string currentPath = path;

        List<Color> referencedColor = new();

        if (_main.Handler.TempPathOverrides.ContainsKey(path)) pathOverride = _main.Handler.TempPathOverrides[path];
        if (pathOverride != path) _main.Handler.TempPathOverrides[path] = pathOverride;

        if (_main.Handler.PathColors.ContainsKey(path))
        {
            foreach (var clip in _main.Handler.AnimationClips)
            {
                if (_main.Handler.ClipColors.ContainsKey(clip))
                {
                    if (_main.Handler.PathColors[path].Contains(clip)) referencedColor.Add(_main.Handler.ClipColors[clip]);
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
    public void ClipPathObjectChanged(EClipNameField clipNameField, GameObject gameObject, GameObject newObject, ref string currentPath)
    {
        gameObject = _main.Handler.FindObjectInRoot(currentPath);

        try
        {
            if (gameObject != newObject)
            {
                //MDebug.Caution($"Start: {gameObject.CheckNull("gameObject")?.name} - {newObject.CheckNull("newObject")?.name} | {clipNameField.CheckNull("clipNameField")?.Name.text} - {currentPath.CheckNull("currentPath")}");

                currentPath = _main.Handler.ChildPath(newObject);
                _main.Handler.UpdatePath(clipNameField.Name.text, currentPath);
                clipNameField.Name.SetText(currentPath);
                gameObject = newObject;
                _main.Handler.FillModel();

                //MDebug.Caution($"End: {gameObject.CheckNull("gameObject")?.name} - {newObject.CheckNull("newObject")?.name} | {clipNameField.CheckNull("clipNameField")?.Name.text} - {currentPath.CheckNull("currentPath")}");

            }
        }
        catch (UnityException)
        {
            MDebug.Caution($"<color=#c7ff96><b>{currentPath}</b></color> already exits in animation!");
            GameObject returnedObject = _main.Handler.FindObjectInRoot(clipNameField.Name.text);
            clipNameField.Object.DragPerformOnField(returnedObject);
        }
        clipNameField.UpdateArrowColor();
    }
    public void ClipPathRootChanged(EClipNameField clipNameField, ref string currentPath, ref string newPath, string setNewPath, Action additionAction)
    {
        newPath = setNewPath;
        _main.Handler.TempPathOverrides.Remove(currentPath);

        try
        {
            if (newPath != currentPath)
            {
                clipNameField.LastRoot = currentPath;

                _main.Handler.UpdatePath(currentPath, newPath);
                currentPath = newPath;

                GameObject getObject = _main.Handler.FindObjectInRoot(currentPath);

                clipNameField.Object.DragPerformOnField(getObject);
                clipNameField.UpdateArrowColor();

                additionAction?.Invoke();

                _main.Handler.FillModel();
            }
        }
        catch (UnityException)
        {
            MDebug.Caution($"<color=#c7ff96><b>{currentPath}</b></color> already exits in animation!");
            clipNameField.Name.SetText(currentPath);
        }
    }
    #endregion
}
#endif