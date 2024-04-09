using UnityEngine.UIElements;

namespace YNL.Editor.UIElement
{
    public class EInputVisualElement : VisualElement, INotifyValueChanged<object>
    {
        public object value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void SetValueWithoutNotify(object newValue)
        {
            throw new System.NotImplementedException();
        }
    }
}