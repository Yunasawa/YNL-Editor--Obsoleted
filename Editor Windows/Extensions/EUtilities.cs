#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using YNL.Extensions.Methods;

namespace YNL.Editors.Windows.Utilities
{
    public static class EUtilities
    {
        public static string ECustom(this string uss, string custom) => $"{uss}__{custom}";
        public static string EHover(this string uss) => $"{uss}__hover";
        public static string EDrag(this string uss) => $"{uss}__drag";

        public static T ELoadAsset<T>(this string path) where T : UnityEngine.Object => Resources.Load<T>(path);
        public static float Remap(this float value, EMinMax origin, EMinMax target)
        {
            float originRate = 100 / (origin.Max - origin.Min);
            float currentPercent = value * originRate;
            return target.Min + (target.Max - target.Min) * currentPercent.Percent();
        }

    }

    public static class EDebug
    {
        public static void ELog(object message) => Debug.Log($"<color=#9EFFF9><b>ⓘ Log:</b></color> {message}");
        public static void EWarning(object message) => Debug.LogWarning($"<color=#FFE045><b>⚠ Warning:</b></color> {message}");
        public static void ECaution(object message) => Debug.Log($"<color=#FF983D><b>⚠ Caution:</b></color> {message}");
        public static void EAction(object message) => Debug.Log($"<color=#EC82FF><b>▶ Action:</b></color> {message}");
        public static void ENotify(object message) => Debug.Log($"<color=#FFCD45><b>▶ Notification:</b></color> {message}");
        public static void EError(object message) => Debug.LogError($"<color=#FF3C2E><b>⚠ Error:</b></color> {message}");
        public static void ECustom(string custom = "Custom", object message = null, string color = "#63ff9a") => Debug.Log($"<color={color}><b>✔ {custom}:</b></color> {message}");
    }
}
#endif