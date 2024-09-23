#if UNITY_EDITOR && YNL_UTILITIES
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using YNL.Extensions.Methods;
using YNL.EditorsObsoleted.Extensions;

namespace YNL.EditorsObsoleted.Windows.AnimationObjectRenamer
{
    [InitializeOnLoad]
    public class Handler
    {
        #region ▶ Fields/Properties
        private Main _main;

        public static Hashtable Paths;
        public static Dictionary<string, string> TempPathOverrides = new();
        public static ArrayList PathKeys = new();

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

        private static void OnGameObjectDestroyed((string path, GameObject obj)[] removedPaths)
        {
            if (!Variable.IsAutomaticOn) return;

            Animator[] animators = new Animator[0];
            bool isExistingInClips = false;
            bool anyExistingInClips = false;
            int clipCount = 0;
            string anyNameMissing = "";

            Undo.PerformUndo();

            PreviousName = removedPaths[0].obj.name;

            foreach (var removedPath in removedPaths)
            {
                isExistingInClips = false;

                if (!removedPath.obj.IsNullOrDestroyed()) animators = GetAnimatorsInParents(removedPath.obj);

                foreach (var animator in animators)
                {
                    AnimationClips = GetAnimationClips(animator).ToList();
                    FillModel();

                    foreach (string path in PathKeys)
                    {
                        if (path.Contains(removedPath.obj.GetAnimationPath(animator)))
                        {
                            isExistingInClips = true;
                            anyExistingInClips = true;

                            if (anyNameMissing.IsNullOrEmpty()) anyNameMissing = removedPath.obj.name;
                            break;
                        }
                    }

                    clipCount += AnimationClips.Count;
                }

                if (!isExistingInClips)
                {
                    if (!removedPath.obj.IsNullOrDestroyed()) Undo.DestroyObjectImmediate(removedPath.obj);
                    HierarchyChangeCatcher.RefreshPreviousKeys();
                }
            }

            if (anyExistingInClips)
            {
                DialogPopup.Open(
                            "ⓘ Missing reference on destroyed objects",
                            $"Attempted to destroy object <color=#d5ff78>{anyNameMissing}</color>, which are animated objects. This can " +
                            $"cause a missing reference error on several animation clips." +
                            $"\n\nDestruction aborted to prevent potential path missing issues and animation disruptions.",
                            "Understand");

                string[] paths = removedPaths.Select(i => i.path.Split('/')[^1]).ToArray();

                AORSettings.AutomaticLog log = new(AORSettings.Event.Destroy, false, paths, paths);
                Variable.AutomaticLogs.Add(log);

                EditorApplication.RepaintAnimationWindow();

                Visual.UpdateLogPanel();
                Variable.SaveData();
            }
        }

        private static void OnGameObjectRenamed((GameObject obj, string name)[] renamedObjects)
        {
            if (!Variable.IsAutomaticOn) return;

            Animator[] animators = new Animator[0];
            bool isDuplicated = false;
            string oldPath = "";
            string newPath = "";
            bool isShowLog = false;
            bool isSucceeded = true;
            int clipCount = 0;
            bool newPathExisted = false;

            string preOldPath = "";

            string[] oldNames = renamedObjects.Select(o => o.name).ToArray();
            string[] newNames = renamedObjects.Select(n => n.obj.name).ToArray();

            foreach (var renamedObject in renamedObjects)
            {
                isDuplicated = renamedObject.obj.HasDuplicatedNameInSamePath();

                if (!renamedObject.obj.IsNullOrDestroyed()) animators = GetAnimatorsInParents(renamedObject.obj);

                foreach (var animator in animators)
                {
                    newPathExisted = false;

                    preOldPath = renamedObject.obj.GetAnimationPath(animator, false);
                    if (preOldPath != "") preOldPath += "/";

                    oldPath = $"{preOldPath}{renamedObject.name}";
                    newPath = renamedObject.obj.GetAnimationPath(animator);

                    AnimationClips = GetAnimationClips(animator).ToList();
                    FillModel();

                    foreach (string path in PathKeys)
                    {
                        if (path.Contains(oldPath)) ValidPaths.Add(path);
                        else if (path.Contains(newPath)) newPathExisted = true;
                    }

                    bool oldPathExisted = !ValidPaths.IsEmpty();

                    if (isDuplicated && (newPathExisted || oldPathExisted))
                    {
                        if (renamedObjects.Length == 1)
                        {
                            DialogPopup.Open(
                                "ⓘ Conflict names in the same path",
                                $"Attempted to rename object <color=#d5ff78>{renamedObject.name}</color> " +
                                $"to <color=#d5ff78>{renamedObject.obj.name}</color>, but the name is already " +
                                $"in use by another object at the same path." +
                                $"\n\nRename operation aborted to prevent naming conflicts.",
                                "Understand");
                        }
                        else
                        {
                            DialogPopup.Open(
                                "ⓘ Conflict names in the same path",
                                $"Attempted to rename object <color=#d5ff78>{renamedObject.name}</color> " +
                                $"to <color=#d5ff78>{renamedObject.obj.name}</color>, but one of them is an animted object." +
                                $"\n\nRename operation aborted to prevent naming conflicts.",
                                "Understand");
                        }

                        if (renamedObjects.Length == 1) renamedObject.obj.name = renamedObject.name;

                        EditorApplication.RepaintAnimationWindow();

                        isSucceeded = false;
                        isShowLog = true;
                    }
                    else
                    {
                        foreach (var path in ValidPaths)
                        {
                            Visual.ReplaceClipPathItem(path, path.Replace(oldPath, newPath), out isSucceeded, true);
                            isShowLog = true;
                        }
                    }

                    clipCount += AnimationClips.Count;

                    AnimationClips.Clear();
                    Paths.Clear();
                    PathKeys.Clear();
                    ValidPaths.Clear();
                    InvalidCount = 0;
                }
            }

            if (!isSucceeded)
            {
                foreach (var renamedObject in renamedObjects) renamedObject.obj.name = renamedObject.name;
            }

            if (isShowLog)
            {
                AORSettings.Event @event = renamedObjects.Length == 1 ? AORSettings.Event.RenameSingle : AORSettings.Event.RenameMultiple;

                AORSettings.AutomaticLog log = new(@event, isSucceeded, oldNames, newNames);
                Variable.AutomaticLogs.Add(log);

                Visual.UpdateLogPanel();
                Variable.SaveData();
            }
        }

        private static void OnGameObjectMoved((GameObject obj, string previous, string current)[] movedObjects)
        {
            if (!Variable.IsAutomaticOn) return;

            Animator[] animators = new Animator[0];
            string oldName = "";
            string oldPath = "";
            string newPath = "";
            bool isValidAnimator = false;
            bool newPathExisted = false;
            bool isSucceeded = true;
            bool isShowLog = false;
            AORSettings.Event @event = AORSettings.Event.MoveSucceed;
            List<string> failedNames = new();

            foreach (var movedObject in movedObjects)
            {
                oldName = movedObject.obj.name;

                GameObject parent = movedObject.previous.GetParent();

                if (!parent.IsNull()) animators = GetAnimatorsInParents(parent);

                foreach (var animator in animators)
                {
                    isValidAnimator = movedObject.current.Contains(animator.GetPath());
                    if (!isValidAnimator) break;
                }

                foreach (var animator in animators)
                {
                    if (isValidAnimator)
                    {
                        AnimationClips = GetAnimationClips(animator).ToList();
                        FillModel();

                        oldPath = movedObject.previous.RemoveAll(animator.gameObject.GetPath()).Substring(1);
                        newPath = movedObject.current.RemoveAll(animator.gameObject.GetPath()).Substring(1);

                        foreach (string path in PathKeys)
                        {
                            if (path.Contains(oldPath)) ValidPaths.Add(path);
                            else if (path.Contains(newPath)) newPathExisted = true;
                        }

                        if (ValidPaths.IsEmpty() && newPathExisted)
                        {
                            DialogPopup.Open(
                                "ⓘ Another object with the same path exists",
                                $"Attempted to move object <color=#d5ff78>{oldName}</color>, " +
                                $"but the destination already contains another object with the same name." +
                                $"\n\nMoving operation aborted to prevent duplicating conflicts.",
                                "Understand");

                            movedObject.obj.transform.SetParent(parent.transform, true);

                            isShowLog = true;
                            isSucceeded = false;

                            @event = AORSettings.Event.MoveConflict;
                            failedNames.AddDistinct(movedObject.obj.name);

                            EditorApplication.RepaintAnimationWindow();
                            HierarchyChangeCatcher.RefreshPreviousKeys();
                        }
                        else
                        {
                            foreach (var path in ValidPaths)
                            {
                                Visual.ReplaceClipPathItem(path, path.Replace(oldPath, newPath), out isSucceeded, true);

                                isShowLog = true;
                                isSucceeded = true;

                                @event = AORSettings.Event.MoveSucceed;

                                failedNames.AddDistinct(movedObject.obj.name);
                            }
                        }
                    }
                    else
                    {
                        DialogPopup.Open(
                                "ⓘ Another object with the same path exists",
                                $"Attempted to move object <color=#d5ff78>{oldName}</color>, " +
                                $"but the destination is outside of the previous animator object." +
                                $"\n\nMoving operation aborted to prevent missing errors.",
                                "Understand");

                        movedObject.obj.transform.SetParent(parent.transform, true);

                        isShowLog = true;
                        isSucceeded = false;

                        @event = AORSettings.Event.MoveOutbound;
                        failedNames.AddDistinct(movedObject.obj.name);

                        EditorApplication.RepaintAnimationWindow();
                        HierarchyChangeCatcher.RefreshPreviousKeys();
                    }
                }

                AnimationClips.Clear();
                Paths.Clear();
                PathKeys.Clear();
                ValidPaths.Clear();
                InvalidCount = 0;
            }

            if (isShowLog)
            {
                AORSettings.AutomaticLog log = new(@event, isSucceeded, failedNames.ToArray(), failedNames.ToArray());
                Variable.AutomaticLogs.Add(log);

                Visual.UpdateLogPanel();
                Variable.SaveData();
            }
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
                            Color bindedColor = MColor.RandomColor(100, 255);
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

                ClipColors.Add((AnimationClip)Selection.activeObject, MColor.RandomColor(100, 255));
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
            PathKeys = new ArrayList();

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
                    PathKeys.Add(key);
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
                for (int iCurrentPath = 0; iCurrentPath < PathKeys.Count; iCurrentPath++)
                {
                    string path = PathKeys[iCurrentPath] as string;
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
                        float fProgress = (iCurrentClip * fChunk) + fChunk * ((float)iCurrentPath / (float)PathKeys.Count);

                        EditorUtility.DisplayProgressBar("Animation Hierarchy Progress", "How far along the animation editing has progressed.", fProgress);
                    }
                }
            }
            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();

            Visual.PresentAllClips(ClipColors);
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
                if (!Variable.IsAutomaticOn) Undo.RecordObject(animationClip, "Animation Hierarchy Root Change");

                for (int iCurrentPath = 0; iCurrentPath < PathKeys.Count; iCurrentPath++)
                {
                    string path = PathKeys[iCurrentPath] as string;
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
                    fProgress = (iCurrentClip * fChunk) + fChunk * ((float)iCurrentPath / (float)PathKeys.Count);

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
            _main.Visual.UpdateAutomatic(true);
            Variable.OnModeChanged?.Invoke();
        }
        #endregion
    }
}
#endif