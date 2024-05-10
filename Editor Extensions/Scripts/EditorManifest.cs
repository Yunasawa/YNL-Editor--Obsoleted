#if UNITY_EDITOR
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
        private static string _manifestPath;
        private static ManifestRoot _manifestRoot = new();

        private static void UpdateManifest()
        {
            _manifestPath = Application.dataPath.Replace("Assets", "Packages/manifest.json");
            _manifestRoot = JsonData.LoadNewtonJson<ManifestRoot>(_manifestPath);
        }

        public static void AddDependency(string name, string version)
        {
            UpdateManifest();

            if (!_manifestRoot.dependencies.ContainsKey(name))
            {
                _manifestRoot.dependencies.Add(name, version);
            }
            else
            {
                _manifestRoot.dependencies[name] = version;
            }

            JsonData.SaveNewtonJson(_manifestRoot, _manifestPath);
        }

        public static void AddRegistry(string name, string url, params string[] scopes)
        {
            UpdateManifest();

            Registry registry = _manifestRoot.scopedRegistries.Find(i => i.name == name);

            if (registry == null)
            {
                _manifestRoot.scopedRegistries.Add(new Registry(name, url, scopes));
            }
            else
            {
                foreach (var scope in scopes) AddScope(scope);
            }

            JsonData.SaveNewtonJson(_manifestRoot, _manifestPath);

            void AddScope(string scope)
            {
                if (!registry.scopes.Contains(scope))
                {
                    registry.scopes.Add(scope);
                }
            }
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