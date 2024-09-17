#if UNITY_EDITOR && YNL_UTILITIES
using System.Linq;
using UnityEditorInternal;
using YNL.Extensions.Methods;

namespace YNL.Editors.Extensions
{
    public static partial class EEditor
    {
        public class Tag
        {
            public static bool TagExists(string tagName) => InternalEditorUtility.tags.Contains(tagName);

            public static void AddTag(string name)
            {
                if (!TagExists(name))
                {
                    InternalEditorUtility.AddTag(name);
                    MDebug.Notify($"Tag <color=#7aa6ff><b>{name}</b></color> has been added");
                }
            }

            public static void RemoveTag(string name)
            {
                if (TagExists(name))
                {
                    InternalEditorUtility.RemoveTag(name);
                    MDebug.Notify($"Tag <color=#7aa6ff><b>{name}</b></color> has been removed");
                }
            }
        }
    }
}
#endif