#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace YNL.Editor.Extension
{
    public static class EUtilities
    {
        public static string ECustom(this string uss, string custom) => $"{uss}__{custom}";
        public static string EHover(this string uss) => $"{uss}__hover";
        public static string EDrag(this string uss) => $"{uss}__drag";

        public static T ETryGet<T>(this T[] array, int index)
        {
            if (array.EIsNullOrEmpty()) return default;
            else return array[index];
        }
        public static bool EIsNull<T>(this IList<T> list) => list == null ? true : false;
        public static bool EIsEmpty<T>(this IList<T> list) => !list.EIsNull() && list.Count <= 0 ? true : false;
        public static bool EIsNullOrEmpty<T>(this IList<T> list) => list.EIsNull() || list.EIsEmpty() ? true : false;
        public static bool EIsNull(this object obj) => obj == null || ReferenceEquals(obj, null) || obj.Equals(null);
        public static string EAddSpaces(this string text, bool preserveAcronyms = true)
        {
            if (preserveAcronyms) return Regex.Replace(text, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
            else return Regex.Replace(text, "(?<=.)(?=[A-Z])", " ");
        }
        public static Color EToColor(this string hex)
        {
            Color color = Color.white;

            string newHex = hex;

            if (hex.Contains("#")) newHex = hex.Replace("#", "");

            try
            {
                if (newHex.Length != 6 && newHex.Length != 8)
                {
                    EDebug.EError("Format Exception: Invalid HEX Color Format!");
                    return color;
                }
                else if (newHex.Length == 6)
                {
                    color.r = newHex.Substring(0, 2).EHexToInt() / (float)255;
                    color.g = newHex.Substring(2, 2).EHexToInt() / (float)255;
                    color.b = newHex.Substring(4, 2).EHexToInt() / (float)255;
                }
                else if (newHex.Length == 8)
                {
                    color.r = newHex.Substring(0, 2).EHexToInt() / (float)255;
                    color.g = newHex.Substring(2, 2).EHexToInt() / (float)255;
                    color.b = newHex.Substring(4, 2).EHexToInt() / (float)255;
                    color.a = newHex.Substring(6, 2).EHexToInt() / (float)255;
                }
            }
            catch (FormatException)
            {
                Debug.Log("Format Exception: Invalid HEX Format.");
            }
            return color;
        }
        public static int EHexToInt(this string hex)
        {
            int output = 0;
            try
            {
                output = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            }
            catch (System.FormatException)
            {
                Console.WriteLine("Format Exception: Invalid HEX Format.");
            }
            return output;
        }
        public static bool EIsNullOrEmpty(this string input) => input == null || input == "" || input.Length < 1 ? true : false;
        public static T ELoadAsset<T>(this string path) where T : UnityEngine.Object => Resources.Load<T>(path);
        public static string EToHex(this Color color) => $"#{(int)(color.r * 255):X2}{(int)(color.g * 255):X2}{(int)(color.b * 255):X2}";
        public static int EToInt(this string input)
        {
            try
            {
                return input.EIsNullOrEmpty() ? 0 : int.Parse(input);
            }
            catch (FormatException)
            {
                return 0;
            }
        }
        public static IList<T> EAddDistinct<T>(this IList<T> list, T element)
        {
            if (!list.Contains(element)) list.Add(element);
            return list;
        }
        public static float ERemap(this float value, EMinMax origin, EMinMax target)
        {
            float originRate = 100 / (origin.Max - origin.Min);
            float currentPercent = value * originRate;
            return target.Min + (target.Max - target.Min) * currentPercent.EPercent();
        }
        public static float EPercent(this float value, float range) => value / range;
        public static float EPercent(this float value) => value / 100;
        public static T[] EAdd<T>(this T[] array, T value)
        {
            List<T> list = array.ToList();
            list.Add(value);
            array = list.ToArray();
            return array;
        }
        public static int EIndexOf<T>(this T[] array, T element) => Array.IndexOf(array, element);
        public static T EParse<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        public static Color ENormalize(this Color color) => new Color(color.r / 255, color.g / 255, color.b / 255, 1);

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