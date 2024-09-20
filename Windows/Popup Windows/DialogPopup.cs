using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editors.Extensions;
using YNL.Editors.Windows;
using YNL.Extensions.Methods;

public class DialogPopup : PopupWindow<DialogPopup>
{
    private const string _styleSheet = "Style Sheets/Windows/Popup Windows/DialogPopup";

    private string _title = "";
    private string _message = "";
    private string _button = "";

    public Label Label;
    public Image Background;
    public Image IconSide;
    public Image TextSide;
    public Label Message;

    public Button Button;

    private bool _startDragging;
    private Vector2 _distance;

    public static void Open(string title, string message, string button)
    {
        DialogPopup popup = Show(title, message, button).SetSize(400, 200).SetAnchor(false);
    }

    protected override void Initialize(params object[] parameters)
    {
        _title = parameters[0].ToString();
        _message = parameters[1].ToString();
        _button = parameters[2].ToString();
    }

    protected override void CreateUI()
    {
        this.rootVisualElement.AddStyle(_styleSheet, ESheet.Font).AddClass("Main");

        Label = new Label(_title).AddClass("Label");
        Label.RegisterCallback<MouseDownEvent>(OnMouseDown);
        Label.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        Label.RegisterCallback<MouseUpEvent>(OnMouseUp);

        IconSide = new Image().AddClass("IconSide");

        Message = new Label(_message).AddClass("Message");
        Button = new Button().SetText(_button).AddClass("Button");
        Button.clicked += Close;

        TextSide = new Image().AddClass("TextSide").AddElements(Message, Button);

        Background = new Image().AddClass("Background").AddElements(IconSide, TextSide);
        Background.RegisterCallback<MouseDownEvent>(OnMouseDown);
        Background.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        Background.RegisterCallback<MouseUpEvent>(OnMouseUp);

        this.rootVisualElement.AddElements(Label, Background);

        this.Button.Focus();
    }

    //private void OnGUI()
    //{
    //    if (_startDragging)
    //    {
    //        Vector2 mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
    //        Vector2 position = _distance + mouse;
    //        this.position = new Rect(position.x, position.y, this.position.width, this.position.height);
    //    }
    //}

    private void OnMouseDown(MouseDownEvent evt)
    {
        Vector2 mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        Vector2 position = new(this.position.x, this.position.y);

        _distance = position - mouse;
        _startDragging = true;

        evt.StopPropagation();
    }
    private void OnMouseMove(MouseMoveEvent evt)
    {
        if (_startDragging)
        {
            Vector2 mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            Vector2 position = _distance + mouse;
            this.position = new Rect(position.x, position.y, this.position.width, this.position.height);
        }

        evt.StopPropagation();
    }
    private void OnMouseUp(MouseUpEvent evt)
    {
        _startDragging = false;
    }
}