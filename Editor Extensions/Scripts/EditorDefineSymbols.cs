#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

namespace YNL.Editors.Extensions
{
    public static class EditorDefineSymbols
    {
        private static List<string> _defineSymbols = new();

        /// <summary> Get all defined symbols </summary>
        public static void GetSymbols()
        {
            _defineSymbols = PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.Standalone).Split(";").ToList();
        }

        /// <summary> Check if a symbol is already exists </summary>
        public static bool HasSymbol(string symbol) => _defineSymbols.Contains(symbol);

        /// <summary> Add a single symbol </summary>
        public static void AddSymbol(string symbol)
        {
            GetSymbols();
            if (HasSymbol(symbol)) return;

            _defineSymbols.Add(symbol);
            PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.Standalone, String.Join(";", _defineSymbols));
        }

        /// <summary> Add multiple symbols </summary>
        public static void AddSymbols(params string[] symbols)
        {
            GetSymbols();
            foreach (var symbol in symbols)
            {
                if (HasSymbol(symbol)) continue;
                _defineSymbols.Add(symbol);
            }
            PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.Standalone, String.Join(";", _defineSymbols));
        }

        /// <summary> Remove a single symbol </summary>
        public static void RemoveSymbol(string symbol)
        {
            GetSymbols();
            if (!HasSymbol(symbol)) return;

            _defineSymbols.Remove(symbol);
            PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.Standalone, String.Join(";", _defineSymbols));
        }

        /// <summary> Remove multiple symbols </summary>
        public static void RemoveSymbols(params string[] symbols)
        {
            GetSymbols();
            foreach (var symbol in symbols)
            {
                if (!HasSymbol(symbol)) continue;
                _defineSymbols.Add(symbol);
            }
            PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.Standalone, String.Join(";", _defineSymbols));
        }
    }
}
#endif