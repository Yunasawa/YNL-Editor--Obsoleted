#if UNITY_EDITOR && YNL_UTILITIES
using UnityEditor;
using UnityEngine.UIElements;
using YNL.Extensions.Methods;
using YNL.Editors.Extensions;

namespace YNL.Editors.UIElements.Plained
{
    public class PlainedIntField : PlainedInputField<int>
    {
        public PlainedIntField(SerializedProperty serializedProperty) : base()
        {
            _field = new IntegerField(serializedProperty.name.AddSpaces()).AddClass("Field", "unity-base-field__aligned");

            Initialize(serializedProperty);
        }
    }
}
#endif