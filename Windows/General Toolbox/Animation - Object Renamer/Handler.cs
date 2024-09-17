#if UNITY_EDITOR && YNL_UTILITIES
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using YNL.Extensions.Methods;
using YNL.Utilities.Extensions;
using YNL.Editors.Extensions;

namespace YNL.Editors.Windows.AnimationObjectRenamer
{
    [InitializeOnLoad]
    public class Handler
    {
        #region ▶ Fields/Properties
        private Main _main;

        public static Hashtable Paths;
        public static Dictionary<string, string> TempPathOverrides = new();
        public static ArrayList PathsKeys = new();

        public static Animator ReferencedAnimator;
        public static List<AnimationClip> AnimationClips = new();

        public static Dictionary<AnimationClip, Color> ClipColors = new();
        public static Dictionary<string, AnimationClip[]> PathColors = new();

        private static string _replacementOldRoot;
        private static string _replacementNewRoot;
        #endregion
        #region ▶ Static Fields/Properties
        public static GameObject SelectedObject;
        public static string PreviousName;
        public static List<string> ValidPaths = new();
        public static uint InvalidCount = 0;
        #endregion

        public Handler(Main main)
        {
            _main = main;
        }
        static Handler()
        {
            EEditor.Event.OnHierarchyObjectRenamed -= OnGameObjectRenamed;
            EEditor.Event.OnHierarchyObjectRenamed += OnGameObjectRenamed;

            EEditor.Event.OnHierarchyObjectDestroyed -= OnGameObjectDestroyed;
            EEditor.Event.OnHierarchyObjectDestroyed += OnGameObjectDestroyed;

            EEditor.Event.OnHierarchyObjectMoved -= OnGameObjectMoved;
            EEditor.Event.OnHierarchyObjectMoved += OnGameObjectMoved;

            EEditor.Event.OnHierarchyObjectCreated -= OnGameObjectCreated;
            EEditor.Event.OnHierarchyObjectCreated += OnGameObjectCreated;
        }

        #region ▶ Event Functions
        private static void OnGameObjectCreated(GameObject obj)
        {
            
        }

        private static void OnGameObjectDestroyed(string oldPath, GameObject obj)
        {
            if (!Variable.IsAutomaticOn) return;

            PreviousName = oldPath.Split('/')[^1];

            bool pass = false;
            int clipCount = 0;

            Undo.PerformUndo();

            Animator[] animators = new Animator[0];
            if (!obj.IsNullOrDestroyed()) animators = GetAnimatorsInParents(obj);

            foreach (var animator in animators)
            {
                AnimationClips = GetAnimationClips(animator).ToList();
                FillModel();

                foreach (string path in PathsKeys)
                {
                    if (path.Contains(obj.GetAnimationPath(animator)))
                    {
                        pass = true;
                        break;
                    }
                }

                clipCount += AnimationClips.Count;
            }

            if (!pass)
            {
                if (!obj.IsNullOrDestroyed()) Undo.DestroyObjectImmediate(obj);
                return;
            }
            else
            {
                EditorUtility.DisplayDialog(
                            "Remove animated GameObject",
                            "You are trying to remove the GameObject which is animated in animation clip(s). This will cause a Missing Reference Exception.",
                            "Understand",
                            "Close");

                string finalName = $"{PreviousName}|...";

                AORSettings.AutomaticLog log = new(AORSettings.Event.Destroy, false, finalName, animators.Length, clipCount, obj);
                Variable.AutomaticLogs.Add(log);

                PreviousName = "";

                Visual.UpdateLogPanel();
                Variable.SaveData();
            }
        }

        private static void OnGameObjectRenamed((GameObject, string)[] tuples)
        {
            if (!Variable.IsAutomaticOn) return;

            GameObject obj = tuples[0].Item1;

            PreviousName = tuples[0].Item2;
            SelectedObject = obj;

            bool isSucceeded = true;
            bool isShowLog = true;
            string objectName = obj.name;

            int clipCount = 0;
            GameObject gameObject = obj;

            Animator[] animators = GetAnimatorsInParents(obj);

            bool duplicated = obj.HasDuplicatedNameInSamePath();
            string parentPath = "";
            (string oldPath, string newPath) animationPath = new();

            foreach (var animator in animators)
            {
                parentPath = obj.GetAnimationPath(animator, false);
                animationPath = new($"{parentPath}/{PreviousName}", $"{parentPath}/{obj.name}");

                AnimationClips = GetAnimationClips(animator).ToList();
                FillModel();

                foreach (string path in PathsKeys)
                {
                    if (path.Contains(animationPath.oldPath)) ValidPaths.Add(path);
                    else if (path.Contains(animationPath.newPath)) InvalidCount++;
                }

                if (isSucceeded)
                {
                    if ((ValidPaths.Count == 0 && InvalidCount > 0 && duplicated) ||
                        (ValidPaths.Count != 0 && duplicated))
                    {
                        if (EditorUtility.DisplayDialog(
                            "Duplicated GameObject's name",
                            "You are trying to rename this GameObject into a new name, which may cause a duplication error in Animation Clips.",
                            "Understand",
                            "Close"))
                        {
                            foreach (var pair in tuples) pair.Item1.name = pair.Item2;
                            isSucceeded = false;
                            isShowLog = true;

                            //HierarchyChangeCatcher.RefreshPreviousKeys();
                        }
                        else
                        {
                            foreach (var pair in tuples) pair.Item1.name = pair.Item2;
                            isSucceeded = false;
                            isShowLog = true;

                            //HierarchyChangeCatcher.RefreshPreviousKeys();
                        }
                    }
                    else if (ValidPaths.Count != 0)
                    {
                        foreach (string path in ValidPaths)
                        {
                            Visual.ReplaceClipPathItem(path, path.Replace(animationPath.oldPath, animationPath.newPath), out isSucceeded, true);
                        }

                        isShowLog = true;
                    }

                    if (ValidPaths.Count == 0 && InvalidCount == 0) isShowLog = false;
                }

                clipCount += AnimationClips.Count;

                AnimationClips.Clear();
                Paths.Clear();
                PathsKeys.Clear();
                ValidPaths.Clear();
                InvalidCount = 0;
            }

            if (isShowLog)
            {
                MDebug.Warning($"{animationPath.oldPath}\n{animationPath.newPath}");

                string finalName = $"{PreviousName}|{objectName}";
                AORSettings.AutomaticLog log = new(AORSettings.Event.Rename, isSucceeded, finalName, animators.Length, clipCount, gameObject);
                Variable.AutomaticLogs.Add(log);

                Visual.UpdateLogPanel();
                Variable.SaveData();
            }
        }

        private static void OnGameObjectMoved((GameObject, string, string)[] gameObject)
        {

        }
        #endregion
        #region ▶ Handle Functions
        public static Animator[] GetAnimatorsInParents(GameObject obj)
        {
            List<Animator> animators = new();
            GameObject currentObject = obj;

            while (true)
            {
                if (currentObject.HasComponent(out Animator animator)) animators.Add(animator);

                if (currentObject == obj.transform.root.gameObject) break;
                currentObject = currentObject.transform.parent.gameObject;
            }

            return animators.ToArray();
        }

        public static AnimationClip[] GetAnimationClips(Animator animator) => animator.runtimeAnimatorController.animationClips;
        public void OnSelectionChange()
        {
            if (Selection.objects.Length > 1)
            {
                foreach (UnityEngine.Object obj in Selection.objects)
                {
                    if (obj is AnimationClip)
                    {
                        if (!AnimationClips.Contains((AnimationClip)obj))
                        {
                            AnimationClips.Add((AnimationClip)obj);
                        }
                        if (!ClipColors.ContainsKey((AnimationClip)obj))
                        {
                            Color bindedColor = new Color(UnityEngine.Random.Range(100, 255), UnityEngine.Random.Range(100, 255), UnityEngine.Random.Range(100, 255), 1).Normalize();
                            ClipColors.Add((AnimationClip)obj, bindedColor);
                        }
                    }
                }

                Visual.PresentAllClips(ClipColors);
                Visual.PresentAllPaths();
            }
            else if (Selection.activeObject is AnimationClip)
            {
                ClipColors.Clear();
                PathColors.Clear();
                AnimationClips.Clear();

                ClipColors.Add((AnimationClip)Selection.activeObject, CColor.MediumSpringGreen);
                AnimationClips.Add((AnimationClip)Selection.activeObject);

                Visual.PresentAllClips(ClipColors);
                Visual.PresentAllPaths();
            }
            else if (Selection.activeObject == null)
            {
                AnimationClips.Clear();
                ClipColors.Clear();
                PathColors.Clear();

                Visual.PresentAllClips(ClipColors);
                Visual.PresentAllPaths();
            }

            _main.Root.Repaint();
        }

        public static void GetReferencedAnimator()
        {
            ReferencedAnimator = Visual.ReferencedAnimator.ReferencedObject;
        }

        public static void FillModel()
        {
            Paths = new Hashtable();
            PathsKeys = new ArrayList();

            foreach (AnimationClip animationClip in AnimationClips)
            {
                FillModelWithCurves(AnimationUtility.GetCurveBindings(animationClip), animationClip);
                FillModelWithCurves(AnimationUtility.GetObjectReferenceCurveBindings(animationClip), animationClip);
            }
        }
        private static void FillModelWithCurves(EditorCurveBinding[] curves, AnimationClip clip)
        {
            foreach (EditorCurveBinding curveData in curves)
            {
                string key = curveData.path;

                if (Paths.ContainsKey(key))
                {
                    ((ArrayList)Paths[key]).Add(curveData);
                }
                else
                {
                    ArrayList newProperties = new ArrayList();
                    newProperties.Add(curveData);
                    Paths.Add(key, newProperties);
                    PathsKeys.Add(key);
                }

                if (PathColors.ContainsKey(key))
                {
                    if (!PathColors[key].Contains(clip))
                    {
                        PathColors[key] = PathColors[key].Add(clip);
                    }
                }
                else
                {
                    PathColors.Add(key, new AnimationClip[1] { clip });
                }
            }
        }
        public static GameObject FindObjectInRoot(Animator animator, string path)
        {
            if (animator == null) return null;

            Transform child = animator.transform.Find(path);

            if (child != null) return child.gameObject;
            else return null;
        }
        public static GameObject FindObjectInRoot(string path) => FindObjectInRoot(ReferencedAnimator, path);

        public static void UpdatePath(string oldPath, string newPath)
        {
            if (Paths[newPath] != null)
            {
                throw new UnityException("Path " + newPath + " already exists in that animation!");
            }
            AssetDatabase.StartAssetEditing();
            for (int iCurrentClip = 0; iCurrentClip < AnimationClips.Count; iCurrentClip++)
            {
                AnimationClip animationClip = AnimationClips[iCurrentClip];
                Undo.RecordObject(animationClip, "Animation Hierarchy Change");

                // Recreating all curves one by one to maintain proper order in the editor - slower than just removing old curve and adding a corrected one, but it's more user-friendly
                for (int iCurrentPath = 0; iCurrentPath < PathsKeys.Count; iCurrentPath++)
                {
                    string path = PathsKeys[iCurrentPath] as string;
                    ArrayList curves = (ArrayList)Paths[path];

                    for (int i = 0; i < curves.Count; i++)
                    {
                        EditorCurveBinding binding = (EditorCurveBinding)curves[i];
                        AnimationCurve curve = AnimationUtility.GetEditorCurve(animationClip, binding);
                        ObjectReferenceKeyframe[] objectReferenceCurve = AnimationUtility.GetObjectReferenceCurve(animationClip, binding);

                        if (curve != null) AnimationUtility.SetEditorCurve(animationClip, binding, null);
                        else AnimationUtility.SetObjectReferenceCurve(animationClip, binding, null);

                        if (path == oldPath) binding.path = newPath;

                        if (curve != null) AnimationUtility.SetEditorCurve(animationClip, binding, curve);
                        else AnimationUtility.SetObjectReferenceCurve(animationClip, binding, objectReferenceCurve);

                        float fChunk = 1f / AnimationClips.Count;
                        float fProgress = (iCurrentClip * fChunk) + fChunk * ((float)iCurrentPath / (float)PathsKeys.Count);

                        EditorUtility.DisplayProgressBar("Animation Hierarchy Progress", "How far along the animation editing has progressed.", fProgress);
                    }
                }
            }
            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();

            Visual.PresentAllClips(ClipColors);
            //_main.Root.Repaint();

            //Paths.Add(newPath, Paths[oldPath]);
            //Paths.Remove(oldPath);
        }
        public static string ChildPath(GameObject obj, bool sep = false)
        {
            if (ReferencedAnimator == null)
            {
                throw new UnityException("Please assign Referenced Animator (Root) first!");
            }

            if (obj == ReferencedAnimator.gameObject) return "";
            else
            {
                if (obj.transform.parent == null) throw new UnityException("Object must belong to " + ReferencedAnimator.ToString() + "!");
                else return ChildPath(obj.transform.parent.gameObject, true) + obj.name + (sep ? "/" : "");
            }
        }
        public static string GetPath(GameObject obj, bool sep = false)
        {
            if (obj.transform.parent == null) return "";
            return GetPath(obj.transform.parent.gameObject, true) + obj.name + (sep ? "/" : "");
        }
        public static void ReplaceRoot(string oldRoot, string newRoot, Action done = null)
        {
            float fProgress = 0.0f;
            _replacementOldRoot = oldRoot;
            _replacementNewRoot = newRoot;

            AssetDatabase.StartAssetEditing();

            for (int iCurrentClip = 0; iCurrentClip < AnimationClips.Count; iCurrentClip++)
            {
                AnimationClip animationClip = AnimationClips[iCurrentClip];
                Undo.RecordObject(animationClip, "Animation Hierarchy Root Change");

                for (int iCurrentPath = 0; iCurrentPath < PathsKeys.Count; iCurrentPath++)
                {
                    string path = PathsKeys[iCurrentPath] as string;
                    ArrayList curves = (ArrayList)Paths[path];

                    for (int i = 0; i < curves.Count; i++)
                    {
                        EditorCurveBinding binding = (EditorCurveBinding)curves[i];

                        if (path == _replacementOldRoot)
                        {
                            if (path != _replacementNewRoot)
                            {
                                string sNewPath = Regex.Replace(path, "^" + _replacementOldRoot, _replacementNewRoot);

                                AnimationCurve curve = AnimationUtility.GetEditorCurve(animationClip, binding);
                                if (curve != null)
                                {
                                    AnimationUtility.SetEditorCurve(animationClip, binding, null);
                                    binding.path = sNewPath;
                                    AnimationUtility.SetEditorCurve(animationClip, binding, curve);
                                }
                                else
                                {
                                    ObjectReferenceKeyframe[] objectReferenceCurve = AnimationUtility.GetObjectReferenceCurve(animationClip, binding);
                                    AnimationUtility.SetObjectReferenceCurve(animationClip, binding, null);
                                    binding.path = sNewPath;
                                    AnimationUtility.SetObjectReferenceCurve(animationClip, binding, objectReferenceCurve);
                                }

                                FillModel();
                            }
                        }
                    }

                    float fChunk = 1f / AnimationClips.Count;
                    fProgress = (iCurrentClip * fChunk) + fChunk * ((float)iCurrentPath / (float)PathsKeys.Count);

                    EditorUtility.DisplayProgressBar(
                        "Animation Hierarchy Progress",
                        "How far along the animation editing has progressed.",
                        fProgress);
                }

            }
            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();
            Visual.PresentAllClips(ClipColors);
            //_main.Root.Repaint();

            done?.Invoke();
        }

        public void ChangeMode(bool toAutomatic)
        {
            Variable.IsAutomaticPanel = toAutomatic;
            _main.Visual.UpdateMode(true);
        }
        public void SwitchAutomaticMode()
        {
            Variable.IsAutomaticOn = !Variable.IsAutomaticOn;
            _main.Visual.UpdateAutomatic();
        }
        #endregion
    }
}
#endif