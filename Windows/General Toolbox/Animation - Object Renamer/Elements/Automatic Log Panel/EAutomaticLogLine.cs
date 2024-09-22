#if UNITY_EDITOR && YNL_UTILITIES
using System;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editors.Extensions;
using YNL.Editors.UIElements.Plained;
using YNL.Extensions.Methods;

namespace YNL.Editors.Windows.AnimationObjectRenamer
{
    public class EAutomaticLogLine : Button
    {
        private const string USS_StyleSheet = "Style Sheets/Windows/General Toolbox/Animation - Object Renamer/EAutomaticLogLine";

        public Image State;
        public Button Time;
        public PlainedEllipsisButton Path;
        public GameObject BindedObject;
        public Button Count;
        public VisualElement PathContainer;

        public EAutomaticLogLine(AORSettings.AutomaticLog log) : base()
        {
            this.AddStyle(USS_StyleSheet, ESheet.Font).AddClass("Root");

            State = new Image().AddClass("State").SetBackgroundColor(log.IsSucceeded ? "#3dffa8" : "#ff4a4a");

            Time = new Button().AddClass("Time").SetText(log.CurrentTime);

            Path = new PlainedEllipsisButton().AddClass("Path").AddClass("OldPath");
            PathContainer = new VisualElement().AddClass("PathContainer").AddElements(Path);

            if (log.Event == AORSettings.Event.Destroy)
            {
                if (!log.OldNames.IsEmpty())
                {
                    for (int i = 0; i < log.OldNames.Length; i++)
                    {
                        log.OldNames[i] = $"<color=#ffef70>{log.OldNames[i].FillSpace(log.OldNames[i].Length, '`')}</color>";
                        log.NewNames[i] = $"<color=#ffef70>{log.NewNames[i].FillSpace(log.NewNames[i].Length, '`')}</color>";
                    }
                }

                if (log.OldNames.Length == 1) Path.SetText($"Can not destroy {log.OldNames[0]} as it is an animated object.");
                else Path.SetText($"Can not destroy {String.Join(", ", log.OldNames)} as they are animated objects.");

                State.SetBackgroundImage("Textures/Icons/Remove").SetBackgroundColor("#ffdb4a");
            }
            else if (log.Event == AORSettings.Event.RenameSingle || log.Event == AORSettings.Event.RenameMultiple)
            {
                if (!log.OldNames.IsEmpty())
                {
                    string header = log.IsSucceeded ? "<color=#73ffc0>" : "<color=#ff7878>";
                    string footer = "</color>";

                    if (!log.OldNames.IsEmpty())
                    {
                        if (log.OldNames.Length == 1)
                        {
                            string[] originOldNames = (string[])log.OldNames.Clone();
                            string[] originNewNames = (string[])log.NewNames.Clone();

                            for (int i = 0; i < log.OldNames.Length; i++)
                            {
                                log.OldNames[i] = log.OldNames[i].HighlightDifferences(originNewNames[i], true, header, footer);
                                log.NewNames[i] = log.NewNames[i].HighlightDifferences(originOldNames[i], true, header, footer);
                            }
                        }

                        for (int i = 0; i < log.OldNames.Length; i++)
                        {
                            log.OldNames[i] = $"<color=#ffef70>{log.OldNames[i].FillSpace(log.OldNames[i].Length, '`')}</color>";
                            log.NewNames[i] = $"<color=#ffef70>{log.NewNames[i].FillSpace(log.NewNames[i].Length, '`')}</color>";
                        }

                        if (log.IsSucceeded)
                        {
                            string oldNames = String.Join(", ", log.OldNames);
                            Path.SetText($"Renamed {oldNames} to {log.NewNames[0]}");
                        }
                        else
                        {
                            string oldNames = String.Join(", ", log.OldNames);
                            if (log.Event == AORSettings.Event.RenameSingle) Path.SetText($"Can not rename {oldNames} to {log.NewNames[0]} as it is taken by another object.");
                            if (log.Event == AORSettings.Event.RenameMultiple) Path.SetText($"Can not rename {oldNames} to {log.NewNames[0]} as one of them is an animated object.");
                        }
                    }

                    State.SetBackgroundImage("Textures/Icons/Rename");
                }
            }
            else if (log.Event == AORSettings.Event.MoveSucceed || log.Event == AORSettings.Event.MoveConflict || log.Event == AORSettings.Event.MoveOutbound)
            {
                MDebug.Log($"Hehe: {log.OldNames.Length}");

                if (!log.OldNames.IsEmpty())
                {
                    for (int i = 0; i < log.OldNames.Length; i++)
                    {
                        log.OldNames[i] = $"<color=#ffef70>{log.OldNames[i].FillSpace(log.OldNames[i].Length, '`')}</color>";
                    }

                    string oldNames = String.Join(", ", log.OldNames);

                    MDebug.Log("Hello");
                    if (log.Event == AORSettings.Event.MoveSucceed)
                    {
                        Path.SetText($"Moving {oldNames} succeeded.");
                        MDebug.Log("Hello");
                    }
                    else if (log.Event == AORSettings.Event.MoveConflict)
                    {
                        Path.SetText($"Can not move {oldNames}, the destination already contains object with the same name.");
                    }
                    else if (log.Event == AORSettings.Event.MoveOutbound)
                    {
                        Path.SetText($"Can not move {oldNames}, the destination is outside of the previous animator object.");
                    }

                    State.SetBackgroundImage("Textures/Icons/Move");
                }
            }

            this.AddElements(State, Time, PathContainer);
        }
    }
}
#endif