#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using YNL.Editor.Extensions;

namespace YNL.Editor.Window.Animation.ObjectRenamer
{
    public class WAnimationObjectRenamer_Handler
    {
        #region ▶ Fields/Properties
        private WAnimationObjectRenamer_Main _main;

        public Hashtable Paths;
        public Dictionary<string, string> TempPathOverrides = new();
        public ArrayList PathsKeys;

        public Animator ReferencedAnimator;
        public List<AnimationClip> AnimationClips = new();

        public Dictionary<AnimationClip, Color> ClipColors = new();
        public Dictionary<string, AnimationClip[]> PathColors = new();

        private string _replacementOldRoot;
        private string _replacementNewRoot;
        #endregion

        public WAnimationObjectRenamer_Handler(WAnimationObjectRenamer_Main main)
        {
            _main = main;
        }

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

                _main.Visual.PresentAllClips(ClipColors);
                _main.Visual.PresentAllPaths();
            }
            else if (Selection.activeObject is AnimationClip)
            {
                ClipColors.Clear();
                PathColors.Clear();
                AnimationClips.Clear();

                ClipColors.Add((AnimationClip)Selection.activeObject, EColor.MediumSpringGreen);
                AnimationClips.Add((AnimationClip)Selection.activeObject);

                _main.Visual.PresentAllClips(ClipColors);
                _main.Visual.PresentAllPaths();
            }
            else if (Selection.activeObject == null)
            {
                AnimationClips.Clear();
                ClipColors.Clear();
                PathColors.Clear();

                _main.Visual.PresentAllClips(ClipColors);
                _main.Visual.PresentAllPaths();
            }

            _main.Root.Repaint();
        }
        public void OnGUI()
        {
            GetReferencedAnimator();
        }

        public void GetReferencedAnimator()
        {
            if (_main.Visual.IsNull()) return;

            ReferencedAnimator = _main.Visual.ReferencedAnimator.ReferencedObject;
        }

        public void FillModel()
        {
            Paths = new Hashtable();
            PathsKeys = new ArrayList();

            foreach (AnimationClip animationClip in AnimationClips)
            {
                FillModelWithCurves(AnimationUtility.GetCurveBindings(animationClip), animationClip);
                FillModelWithCurves(AnimationUtility.GetObjectReferenceCurveBindings(animationClip), animationClip);
            }
        }
        private void FillModelWithCurves(EditorCurveBinding[] curves, AnimationClip clip)
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
        public GameObject FindObjectInRoot(string path)
        {
            if (ReferencedAnimator == null) return null;

            Transform child = ReferencedAnimator.transform.Find(path);

            if (child != null) return child.gameObject;
            else return null;
        }

        public void UpdatePath(string oldPath, string newPath)
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

            _main.Visual.PresentAllClips(ClipColors);
            _main.Root.Repaint();

            //Paths.Add(newPath, Paths[oldPath]);
            //Paths.Remove(oldPath);
        }
        public string ChildPath(GameObject obj, bool sep = false)
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
        public void ReplaceRoot(string oldRoot, string newRoot, Action done = null)
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
            _main.Visual.PresentAllClips(ClipColors);
            _main.Root.Repaint();

            done?.Invoke();
        }
    }
}
#endif