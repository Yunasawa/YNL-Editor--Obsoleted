#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using YNL.Extensions.Methods;
using static PlasticPipe.Server.MonitorStats;

namespace YNL.Editors.Extensions
{
    [InitializeOnLoad]
    public class HierarchyChangeCatcher
    {
        public static bool IsInPrefabMode => !PrefabStageUtility.GetCurrentPrefabStage().IsNull();

        public static List<GameObjectKey> PreviousKeys = new();
        public static List<GameObjectKey> CurrentKeys = new();

        private static List<string> previousPaths = new();
        private static List<string> currentPaths = new();

        static HierarchyChangeCatcher()
        {
            EditorSceneManager.sceneOpened -= OnSceneOpened;
            EditorSceneManager.sceneOpened += OnSceneOpened;

            PrefabStage.prefabStageOpened -= OnPrefabOpened;
            PrefabStage.prefabStageOpened += OnPrefabOpened;

            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
            EditorApplication.hierarchyChanged += OnHierarchyChanged;

            RefreshPreviousKeys();
        }

        private static void OnSceneOpened(Scene scene, OpenSceneMode mode) => GetAllObjectsOnScene(PreviousKeys);
        private static void OnPrefabOpened(PrefabStage stage) => GetAllObjectsOnPrefab(PreviousKeys, stage);
        private static void GetChildren(GameObject gameObject, List<GameObjectKey> list)
        {
            foreach (Transform child in gameObject.transform)
            {
                list.Add(new(child.gameObject));
                GetChildren(child.gameObject, list);
            }
        }
        private static void GetAllObjectsOnScene(List<GameObjectKey> list)
        {
            GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            list.Clear();
            foreach (var obj in rootObjects)
            {
                list.Add(new(obj));
                GetChildren(obj, list);
            }
        }
        private static void GetAllObjectsOnPrefab(List<GameObjectKey> list, PrefabStage stage)
        {
            list.Clear();

            if (stage != null)
            {
                list.Add(new(stage.prefabContentsRoot));
                GetChildren(stage.prefabContentsRoot, list);
            }
            else return;
        }

        private static void OnHierarchyChanged()
        {
            if (IsInPrefabMode) GetAllObjectsOnPrefab(CurrentKeys, PrefabStageUtility.GetCurrentPrefabStage());
            else GetAllObjectsOnScene(CurrentKeys);

            if (PreviousKeys.Count == CurrentKeys.Count - 1) // Create multiple object
            {
                for (int i = 0; i < CurrentKeys.Count; i++)
                {
                    if (i == PreviousKeys.Count || PreviousKeys[i] != CurrentKeys[i])
                    {
                        EEditor.Event.OnHierarchyObjectCreated?.Invoke(CurrentKeys[i].Object);
                        RefreshPreviousKeys();
                        break;
                    }
                }
            }
            else if (PreviousKeys.Count < CurrentKeys.Count - 1) // Create multiple objects
            {
                MDebug.Warning("Created multiple objects");

                RefreshPreviousKeys();
            }
            else if (PreviousKeys.Count > CurrentKeys.Count) // Destroy multiple objects
            {
                (string previousPath, GameObject previousObject)[] removedPaths = PreviousKeys
                    .Except(CurrentKeys, new GameObjectKeyComparer())
                    .Select(e => (e.Path, e.Object)).ToArray();

                EEditor.Event.OnHierarchyObjectDestroyed?.Invoke(removedPaths);
            }
            else
            {
                previousPaths.Clear();
                currentPaths.Clear();

                List<(GameObject, string)> renamedObjects = new();

                for (int i = 0; i < PreviousKeys.Count; i++)
                {
                    if (PreviousKeys[i] != CurrentKeys[i])
                    {
                        if (PreviousKeys[i].Object == CurrentKeys[i].Object)
                        {
                            //Rename
                            string previousName = PreviousKeys[i].Path.Split('/')[^1];
                            renamedObjects.Add(new(CurrentKeys[i].Object, previousName));
                            continue;
                        }
                        else
                        {
                            previousPaths.Add(PreviousKeys[i].Path);
                            currentPaths.Add(CurrentKeys[i].Path);
                        }
                    }
                }

                //Rename
                if (renamedObjects.Count > 0)
                {
                    EEditor.Event.OnHierarchyObjectRenamed?.Invoke(renamedObjects.ToArray());
                    RefreshPreviousKeys();
                    return;
                }

                // Moved
                if (previousPaths.IsEmpty() || currentPaths.IsEmpty()) return;

                previousPaths.RemoveAll(path => currentPaths.Remove(path));

                for (int i = 0; i < currentPaths.Count; i++)
                {
                    if (!previousPaths[i].HasParentChanged(currentPaths[i]))
                    {
                        previousPaths.RemoveAt(i);
                        currentPaths.RemoveAt(i--);
                    }
                }

                foreach (var path in previousPaths) MDebug.Action(path);
                foreach (var path in currentPaths) MDebug.Notify(path);

                RefreshPreviousKeys();
            }
        }
        public static void RefreshPreviousKeys()
        {
            if (IsInPrefabMode) OnPrefabOpened(PrefabStageUtility.GetCurrentPrefabStage());
            else OnSceneOpened(SceneManager.GetActiveScene(), OpenSceneMode.Single);

            //MDebug.Log($"PreviousKeys: {PreviousKeys.Count} - CurrentKeys: {CurrentKeys.Count}");
        }
    }

    [System.Serializable]
    public struct GameObjectKey
    {
        public GameObject Object;
        public string Path;
        public int Index;

        public GameObjectKey(GameObject @object)
        {
            Object = @object;
            Path = @object.GetPath(true);
            Index = @object.transform.GetSiblingIndex();
        }

        public override int GetHashCode() => base.GetHashCode();
        public override bool Equals(object obj) => base.Equals(obj);

        public static bool operator ==(GameObjectKey key1, GameObjectKey key2) => key1.Path == key2.Path; 
        public static bool operator !=(GameObjectKey key1, GameObjectKey key2) => key1.Path != key2.Path; 
    }

    public class GameObjectKeyComparer : IEqualityComparer<GameObjectKey>
    {
        public bool Equals(GameObjectKey x, GameObjectKey y) => x.Path == y.Path;

        public int GetHashCode(GameObjectKey obj) => obj.Path.GetHashCode();
    }
}
#endif