#if UNITY_EDITOR && YNL_UTILITIES
using UnityEditor;
using UnityEngine.UIElements;
using YNL.Extensions.Methods;
using YNL.Editors.Windows.Utilities;

namespace YNL.Editors.UIElements.Plained
{
    public class PlainedFloatField : PlainedInputField<float>
    {
        public PlainedFloatField(SerializedProperty serializedProperty) : base()
        {
            _field = new FloatField(serializedProperty.name.AddSpaces()).AddClass("Field", "unity-base-field__aligned");

            Initialize(serializedProperty);
        }
    }
}
#endif