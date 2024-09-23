#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using YNL.Extensions.Methods;

namespace YNL.EditorsObsoleted.Extensions
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
                //MDebug.Warning("Created multiple objects");

                RefreshPreviousKeys();
            }
            else if (PreviousKeys.Count > CurrentKeys.Count) // Destroy single & multiple objects
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
                List<GameObject> changedObjects = new();

                for (int i = 0; i < PreviousKeys.Count; i++)
                {
                    if (PreviousKeys[i].Path.HasBeenRenamed(CurrentKeys[i].Path))
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
                            //Move
                            previousPaths.Add(PreviousKeys[i].Path);
                            currentPaths.Add(CurrentKeys[i].Path);
                            changedObjects.Add(PreviousKeys[i].Object);
                        }
                    }
                    else if (PreviousKeys[i].Path.HasParentChanged(CurrentKeys[i].Path))
                    {
                        //Move
                        previousPaths.Add(PreviousKeys[i].Path);
                        currentPaths.Add(CurrentKeys[i].Path);
                        changedObjects.Add(PreviousKeys[i].Object);
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

                for (int i = previousPaths.Count - 1; i >= 0; i--)
                {
                    if (currentPaths.Remove(previousPaths[i]))
                    {
                        previousPaths.RemoveAt(i);
                        changedObjects.RemoveAt(i);
                    }
                }

                for (int i = 0; i < currentPaths.Count; i++)
                {
                    if (!previousPaths[i].HasParentChanged(currentPaths[i]))
                    {
                        previousPaths.RemoveAt(i);
                        changedObjects.RemoveAt(i);
                        currentPaths.RemoveAt(i--);
                    }
                }

                (GameObject, string, string)[] movedObjects = changedObjects.Select((obj, i) => (obj, previousPaths[i], currentPaths[i])).ToArray();

                if (movedObjects.Length > 0)
                {
                    EEditor.Event.OnHierarchyObjectMoved?.Invoke(movedObjects);
                    RefreshPreviousKeys();
                    return;
                }

                RefreshPreviousKeys();
            }
        }
        public static void RefreshPreviousKeys()
        {
            if (IsInPrefabMode) OnPrefabOpened(PrefabStageUtility.GetCurrentPrefabStage());
            else OnSceneOpened(SceneManager.GetActiveScene(), OpenSceneMode.Single);
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