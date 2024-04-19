namespace YNL.Editor.Window
{
    public interface IMain
    {
        public virtual void OnSelectionChange() { }
        public virtual void CreateGUI() { }
        public virtual void OnGUI() { }

        public virtual void OpenInstruction() { }
    }
}
