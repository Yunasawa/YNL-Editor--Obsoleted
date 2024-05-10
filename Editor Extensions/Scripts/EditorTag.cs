#if UNITY_EDITOR
using System.Linq;
using UnityEditorInternal;
#if YNL_UTILITIES
using YNL.Extensions.Methods;
#endif

namespace YNL.Editors.Extensions
{
    public static class EditorTag
    {
        public static bool TagExists(string tagName) => InternalEditorUtility.tags.Contains(tagName);

        public static void AddTag(string name)
        {
            if (!TagExists(name))
            {
                InternalEditorUtility.AddTag(name);
#if YNL_UTILITIES
                MDebug.Notify($"Tag <color=#7aa6ff><b>{name}</b></color> has been added");
#endif
            }
        }

        public static void RemoveTag(string name)
        {
            if (TagExists(name))
            {
                InternalEditorUtility.RemoveTag(name);
#if YNL_UTILITIES
                MDebug.Notify($"Tag <color=#7aa6ff><b>{name}</b></color> has been removed");
#endif
            }
        }
    }
}
#endif