#if false
using System.Collections.Generic;
using System.IO;
using System;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

namespace YNL.Editors.Extensions
{
    public static class EditorManifest
    {
        private static string _toJsonPath;

        private static ManifestRoot _root = new();

        public static void Test()
        {
            _toJsonPath = Application.dataPath.Replace("Assets", "Packages/manifest.json");

            _root = JsonData.LoadNewtonJson<ManifestRoot>(_toJsonPath);

            if (!_root.dependencies.ContainsKey("com.yunasawa.ynl.editor"))
            {
                _root.dependencies.Add("com.yunasawa.ynl.editor", "1.3.3");

                Registry registry = _root.scopedRegistries.Find(i => i.name == "YunasawaStudio");
                if (registry == null)
                {
                    _root.scopedRegistries.Add(new Registry
                        (
                            "YunasawaStudio",
                            "https://package.openupm.com",
                            "com.yunasawa.ynl.editor",
                            "com.yunasawa.ynl.utilities"
                        ));
                }
                else
                {
                    if (!registry.scopes.Contains("com.yunasawa.ynl.utilities"))
                    {
                        registry.scopes.Add("com.yunasawa.ynl.editor");
                    }
                }
            }
            if (!_root.dependencies.ContainsKey("com.yunasawa.ynl.utilities"))
            {
                _root.dependencies.Add("com.yunasawa.ynl.utilities", "1.2.1");

                Registry registry = _root.scopedRegistries.Find(i => i.name == "YunasawaStudio");
                if (registry == null)
                {
                    _root.scopedRegistries.Add(new Registry
                        (
                            "YunasawaStudio",
                            "https://package.openupm.com",
                            "com.yunasawa.ynl.editor",
                            "com.yunasawa.ynl.utilities"
                        ));
                }
                else
                {
                    if (!registry.scopes.Contains("com.yunasawa.ynl.utilities"))
                    {
                        registry.scopes.Add("com.yunasawa.ynl.utilities");
                    }
                }
            }

            JsonData.SaveNewtonJson(_root, _toJsonPath);
        }

        public static void AddDependency(string name, string version)
        {

        }

        public static void AddRegistry()
        {

        }
    }

    [System.Serializable]
    public class ManifestRoot
    {
        public Dictionary<string, string> dependencies = new();
        public List<Registry> scopedRegistries = new();
    }

    [System.Serializable]
    public class Registry
    {
        public string name;
        public string url;
        public List<string> scopes = new();

        public Registry(string name, string url, params string[] scopes)
        {
            this.name = name;
            this.url = url;
            this.scopes = scopes.ToList();
        }
    }

    public static class JsonData
    {
        public static bool SaveNewtonJson<T>(this T data, string path, Action saveDone = null)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
                Debug.LogWarning("Target json file doesn't exist! Created a new file.");
            }

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, json);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            return true;
        }

        public static T LoadNewtonJson<T>(string path, Action<T> complete = null, Action<string> fail = null)
        {
            T data = JsonConvert.DeserializeObject<T>("");

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                data = JsonConvert.DeserializeObject<T>(json);

                complete?.Invoke(data);
            }
            else
            {
                fail?.Invoke("Load Json Failed!");
            }

            return data;
        }
    }
}
#endif