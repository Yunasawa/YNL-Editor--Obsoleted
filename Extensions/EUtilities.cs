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
        public static string Custom(this string uss, string custom) => $"{uss}__{custom}";
        public static string Hover(this string uss) => $"{uss}__hover";
        public static string Drag(this string uss) => $"{uss}__drag";

        public static T TryGet<T>(this T[] array, int index)
        {
            if (array.IsNullOrEmpty()) return default;
            else return array[index];
        }
        public static bool IsNull<T>(this IList<T> list) => list == null ? true : false;
        public static bool IsEmpty<T>(this IList<T> list) => !list.IsNull() && list.Count <= 0 ? true : false;
        public static bool IsNullOrEmpty<T>(this IList<T> list) => list.IsNull() || list.IsEmpty() ? true : false;
        public static bool IsNull(this object obj) => obj == null || ReferenceEquals(obj, null) || obj.Equals(null);
        public static string AddSpaces(this string text, bool preserveAcronyms = true)
        {
            if (preserveAcronyms) return Regex.Replace(text, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
            else return Regex.Replace(text, "(?<=.)(?=[A-Z])", " ");
        }
        public static Color ToColor(this string hex)
        {
            Color color = Color.white;

            string newHex = hex;

            if (hex.Contains("#")) newHex = hex.Replace("#", "");

            try
            {
                if (newHex.Length != 6 && newHex.Length != 8)
                {
                    MDebug.Error("Format Exception: Invalid HEX Color Format!");
                    return color;
                }
                else if (newHex.Length == 6)
                {
                    color.r = newHex.Substring(0, 2).HexToInt() / (float)255;
                    color.g = newHex.Substring(2, 2).HexToInt() / (float)255;
                    color.b = newHex.Substring(4, 2).HexToInt() / (float)255;
                }
                else if (newHex.Length == 8)
                {
                    color.r = newHex.Substring(0, 2).HexToInt() / (float)255;
                    color.g = newHex.Substring(2, 2).HexToInt() / (float)255;
                    color.b = newHex.Substring(4, 2).HexToInt() / (float)255;
                    color.a = newHex.Substring(6, 2).HexToInt() / (float)255;
                }
            }
            catch (FormatException)
            {
                Debug.Log("Format Exception: Invalid HEX Format.");
            }
            return color;
        }
        public static int HexToInt(this string hex)
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
        public static bool IsNullOrEmpty(this string input) => input == null || input == "" || input.Length < 1 ? true : false;
        public static T LoadAsset<T>(this string path) where T : UnityEngine.Object => Resources.Load<T>(path);
        public static string ToHex(this Color color) => $"#{(int)(color.r * 255):X2}{(int)(color.g * 255):X2}{(int)(color.b * 255):X2}";
        public static int ToInt(this string input)
        {
            try
            {
                return input.IsNullOrEmpty() ? 0 : int.Parse(input);
            }
            catch (FormatException)
            {
                return 0;
            }
        }
        public static IList<T> AddDistinct<T>(this IList<T> list, T element)
        {
            if (!list.Contains(element)) list.Add(element);
            return list;
        }
        public static float Map(this float value, EMinMax origin, EMinMax target)
        {
            float originRate = 100 / (origin.Max - origin.Min);
            float currentPercent = value * originRate;
            return target.Min + (target.Max - target.Min) * currentPercent.Percent();
        }
        public static float Percent(this float value, float range) => value / range;
        public static float Percent(this float value) => value / 100;
        public static T[] Add<T>(this T[] array, T value)
        {
            List<T> list = array.ToList();
            list.Add(value);
            array = list.ToArray();
            return array;
        }
        public static int IndexOf<T>(this T[] array, T element) => Array.IndexOf(array, element);
        public static T Parse<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        public static Color Normalize(this Color color) => new Color(color.r / 255, color.g / 255, color.b / 255, 1);

    }

    public static class MDebug
    {
        public static void Log(object message) => Debug.Log($"<color=#9EFFF9><b>ⓘ Log:</b></color> {message}");
        public static void Warning(object message) => Debug.LogWarning($"<color=#FFE045><b>⚠ Warning:</b></color> {message}");
        public static void Caution(object message) => Debug.Log($"<color=#FF983D><b>⚠ Caution:</b></color> {message}");
        public static void Action(object message) => Debug.Log($"<color=#EC82FF><b>▶ Action:</b></color> {message}");
        public static void Notify(object message) => Debug.Log($"<color=#FFCD45><b>▶ Notification:</b></color> {message}");
        public static void Error(object message) => Debug.LogError($"<color=#FF3C2E><b>⚠ Error:</b></color> {message}");
        public static void Custom(string custom = "Custom", object message = null, string color = "#63ff9a") => Debug.Log($"<color={color}><b>✔ {custom}:</b></color> {message}");
    }
}
#endif