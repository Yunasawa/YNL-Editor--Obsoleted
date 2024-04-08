#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editor.Utilities;

public class EComponentField<T> : Button where T : Component
{
    private const string USS_StyleSheet = EAddress.FolderPath + "Elements/E ObjectField/EObjectField.uss";

    private const string _uss_root = "root";
    private const string _uss_background = "background";
    private const string _uss_icon = "icon";
    private const string _uss_name = "name";
    private const string _uss_ping = "ping";
    private const string _uss_pingicon = "ping-icon";

    public EInteractableComponentField<T> Background;
    public Image Icon;
    public Label Name;
    public EInteractableImage Ping;
    public Image PingIcon;

    public T ReferencedObject;
    private string _typeName;

    /// <param name="bindedObject"> Object that will be binded with referenced object. </param>
    public EComponentField(T bindedObject) : base()
    {
        ReferencedObject = bindedObject;
        _typeName = typeof(T).Name;

        this.AddStyle(USS_StyleSheet).AddClass(_uss_root);

        Texture2D objectIcon = ETexture.Unity(_typeName);
        Icon = new Image().SetBackgroundImage(objectIcon).AddClass(_uss_icon);

        Name = new Label().AddClass(_uss_name);
        if (!ReferencedObject.IsNull()) HighlineField();
        else LowlineField();

        Background = new EInteractableComponentField<T>().AddClass(_uss_background);
        Background.OnPointerDown += PointerDownOnField;
        Background.OnPointerEnter += PointerEnterOnField;
        Background.OnPointerExit += PointerExitOnField;
        Background.OnDragEnter += DragEnterOnField;
        Background.OnDragExit += DragExitOnField;
        Background.OnDragPerform += DragPerformOnField;

        Background.AddElements(Icon, Name);

        PingIcon = new Image().AddClass(_uss_pingicon);

        Ping = new EInteractableImage().AddClass(_uss_ping).AddElements(PingIcon);
        Ping.OnPointerDown += PointerDownOnSelection;
        Ping.OnPointerEnter += PointerEnterOnSelection;
        Ping.OnPointerExit += PointerExitOnSelection;

        this.AddElements(Background, Ping);
    }

    public void OnGUI()
    {
        if (Event.current.commandName == "ObjectSelectorUpdated")
        {
            ReferencedObject = (T)EditorGUIUtility.GetObjectPickerObject();
        }
    }

    public void PointerDownOnField()
    {
        EditorGUIUtility.PingObject(ReferencedObject);
    }
    public void PointerEnterOnField()
    {
        Background.EnableClass(_uss_background.Hover());
        Icon.EnableClass(_uss_icon.Hover());
        Name.EnableClass(_uss_name.Hover());
        Ping.EnableClass($"{_uss_ping}__field-hover");
    }    
    public void PointerExitOnField()
    {
        Background.DisableClass(_uss_background.Hover());
        Icon.DisableClass(_uss_icon.Hover());
        Name.DisableClass(_uss_name.Hover());
        Ping.DisableClass($"{_uss_ping}__field-hover");
    }
    public void DragEnterOnField()
    {
        Ping.DisableClass($"{_uss_ping}__field-hover");
        Background.DisableClass(_uss_background.Hover());
        Background.EnableClass(_uss_background.Drag());
        Ping.EnableClass(_uss_ping.Drag());
    }
    public void DragExitOnField()
    {
        Background.DisableClass(_uss_background.Drag());
        Ping.DisableClass(_uss_ping.Drag());
    }
    public void DragPerformOnField(T referencedObject)
    {
        Background.DisableClass(_uss_background.Drag());
        Ping.DisableClass(_uss_ping.Drag());

        ReferencedObject = referencedObject;
            
        HighlineField();
    }

    public void PointerDownOnSelection()
    {
        EditorGUIUtility.ShowObjectPicker<T>(null, true, "", 0);
    }
    public void PointerEnterOnSelection()
    {
        Ping.EnableClass(_uss_ping.Hover());
        PingIcon.EnableClass(_uss_pingicon.Hover());
    }
    public void PointerExitOnSelection()
    {
        Ping.DisableClass(_uss_ping.Hover());
        PingIcon.DisableClass(_uss_pingicon.Hover());
    }

    private void HighlineField()
    {
        Name.text = $"{ReferencedObject.name} ({_typeName.AddSpaces()})";
        Name.SetColor("#FFFFFF");
        Icon.SetBackgroundImageTintColor("#FFFFFF".ToColor());
    }
    private void LowlineField()
    {
        Name.text = $"None ({_typeName.AddSpaces()})";
        Name.SetColor("#7D7D7D");
        Icon.SetBackgroundImageTintColor("#7F7F7F".ToColor());
    }
}
#endif