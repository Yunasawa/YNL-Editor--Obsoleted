namespace YNL.Editor.Window
{
    public interface IWindow
    {
        virtual void OnSelectionChange() { }
        virtual void CreateGUI() { }
        virtual void OnGUI() { }
    }
}
