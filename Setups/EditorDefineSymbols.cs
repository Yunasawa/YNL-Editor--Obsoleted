#if UNITY_EDITOR
using System.Collections.Generic;
using System;
using UnityEditor.Build;
using UnityEditor;
using System.Linq;
using UnityEngine;

namespace YNL.Editors.Setups
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
            NotifySymbols(symbol, true);
        }

        /// <summary> Add multiple symbols </summary>
        public static void AddSymbols(params string[] symbols)
        {
            GetSymbols();
            foreach (var symbol in symbols)
            {
                if (HasSymbol(symbol)) continue;
                _defineSymbols.Add(symbol);
                NotifySymbols(symbol, true);
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
            NotifySymbols(symbol, false);
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
            NotifySymbols(String.Join("; ", symbols), false);
        }

        /// <summary>  Print a notification into Console panel </summary>
        public static void NotifySymbols(string message, bool isAdded)
        {
            if (isAdded) Debug.Log($"<color=#FFCD45><b>▶ Notification:</b></color> A new define symbol <color=#ffdb7a><b>{message}</b></color> is added.");
            else Debug.Log($"<color=#FFCD45><b>▶ Notification:</b></color> A new define symbol <color=#ffdb7a><b>{message}</b></color> is removed.");
        }
    }
}
#endif