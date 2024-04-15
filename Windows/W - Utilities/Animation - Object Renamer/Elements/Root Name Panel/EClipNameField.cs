#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editor.Extension;
using YNL.Editor.UIElement;
using YNL.Editor.Utility;

namespace YNL.Editor.Window.Animation.ObjectRenamer
{
    public class EClipNameField : Button
    {
        private const string USS_StyleSheet = "Style Sheets/Windows/W - Utilities/Animation - Object Renamer/EClipNameField";

        public VisualElement ColorContainer;
        public VisualElement ObjectContainer;
        public Image NameField;
        public TextField Name;
        public Image Arrow;
        public Image ObjectField;
        public EAssetField<GameObject> Object;

        public EInteractableButton ChangeButton;
        public Image ChangeIcon;

        public EInteractableButton UndoButton;
        public Image UndoIcon;

        public string LastRoot = "";

        public EClipNameField(string path, GameObject gameObject, Color[] colors, Color arrowColor, Action action) : base()
        {
            this.AddStyle(USS_StyleSheet, EAddress.USSFont).AddClass("Root");

            ColorContainer = new VisualElement().AddClass("ColorContainer");

            foreach (var color in colors)
            {
                ColorContainer.AddElements(new Image().SetBackgroundColor(color).AddClass("Color"));
            }
            ColorContainer.AddElements(new Image().AddClass("ColorSpace"));

            Name = new TextField().AddClass("Name").SetText(path);
            NameField = new Image().AddClass("NameField").AddElements(Name);

            Arrow = new Image().SetBackgroundImageTintColor(arrowColor).AddClass("Arrow");

            Object = new EAssetField<GameObject>(gameObject).AddClass("Object");
            ObjectField = new Image().AddClass("ObjectField").AddElements(Object);

            ChangeIcon = new Image().AddClass("ChangeIcon");
            ChangeButton = new EInteractableButton().AddClass("ChangeButton").AddElements(ChangeIcon);
            ChangeButton.OnPointerEnter += () => HoverOnChangeButton(true);
            ChangeButton.OnPointerExit += () => HoverOnChangeButton(false);
            ChangeButton.OnPointerDown += action;

            UndoIcon = new Image().AddClass("UndoIcon");
            UndoButton = new EInteractableButton().AddClass("UndoButton").AddElements(UndoIcon);
            UndoButton.OnPointerEnter += () => HoverOnUndoButton(true);
            UndoButton.OnPointerExit += () => HoverOnUndoButton(false);
            UndoButton.OnPointerDown += action;

            ObjectContainer = new Image().AddClass("ObjectContainer").AddElements(NameField, Arrow, ObjectField, ChangeButton, UndoButton);

            this.AddElements(ColorContainer, ObjectContainer);
        }

        public void UpdateArrowColor()
        {
            Color arrowColor = "#BF4040".ToColor();
            if (!Object.ReferencedObject.IsNull()) arrowColor = "#40BF8F".ToColor();

            this.Arrow.SetBackgroundImageTintColor(arrowColor);
        }

        public void HoverOnChangeButton(bool isHover)
        {
            ChangeButton.EnableClass(isHover, "ChangeButton".Hover());
            ChangeIcon.EnableClass(isHover, "ChangeIcon".Hover());
        }
        public void HoverOnUndoButton(bool isHover)
        {
            UndoButton.EnableClass(isHover, "UndoButton".Hover());
            UndoIcon.EnableClass(isHover, "UndoIcon".Hover());
        }
    }
}
#endif