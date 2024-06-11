#if UNITY_EDITOR && YNL_UTILITIES
using System;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editors.Windows.Utilities;
using YNL.Editors.UIElements.Styled;

namespace YNL.Editors.Windows.Animation.ObjectRenamer
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
        public StyledAssetField<GameObject> Object;

        public StyledInteractableButton ChangeButton;
        public Image ChangeIcon;

        public StyledInteractableButton UndoButton;
        public Image UndoIcon;

        public string LastRoot = "";

        public EClipNameField(string path, GameObject gameObject, Color[] colors, Color arrowColor, Action action) : base()
        {
            this.AddStyle(USS_StyleSheet, EStyleSheet.Font).AddClass("Root");

            ColorContainer = new VisualElement().AddClass("ColorContainer");

            foreach (var color in colors)
            {
                ColorContainer.AddElements(new Image().SetBackgroundColor(color).AddClass("Color"));
            }
            ColorContainer.AddElements(new Image().AddClass("ColorSpace"));

            Name = new TextField().AddClass("Name").SetText(path);
            NameField = new Image().AddClass("NameField").AddElements(Name);

            Arrow = new Image().SetBackgroundImageTintColor(arrowColor).AddClass("Arrow");

            Object = new StyledAssetField<GameObject>(gameObject).AddClass("Object");
            ObjectField = new Image().AddClass("ObjectField").AddElements(Object);

            ChangeIcon = new Image().AddClass("ChangeIcon");
            ChangeButton = new StyledInteractableButton().AddClass("ChangeButton").AddElements(ChangeIcon);
            ChangeButton.OnPointerEnter += () => HoverOnChangeButton(true);
            ChangeButton.OnPointerExit += () => HoverOnChangeButton(false);
            ChangeButton.OnPointerDown += action;

            UndoIcon = new Image().AddClass("UndoIcon");
            UndoButton = new StyledInteractableButton().AddClass("UndoButton").AddElements(UndoIcon);
            UndoButton.OnPointerEnter += () => HoverOnUndoButton(true);
            UndoButton.OnPointerExit += () => HoverOnUndoButton(false);
            UndoButton.OnPointerDown += action;

            ObjectContainer = new Image().AddClass("ObjectContainer").AddElements(NameField, Arrow, ObjectField, ChangeButton, UndoButton);

            this.AddElements(ColorContainer, ObjectContainer);
        }

        public void UpdateArrowColor()
        {
            Color arrowColor = "#BF4040".EToColor();
            if (!Object.ReferencedObject.EIsNull()) arrowColor = "#40BF8F".EToColor();

            this.Arrow.SetBackgroundImageTintColor(arrowColor);
        }

        public void HoverOnChangeButton(bool isHover)
        {
            ChangeButton.EnableClass(isHover, "ChangeButton".EHover());
            ChangeIcon.EnableClass(isHover, "ChangeIcon".EHover());
        }
        public void HoverOnUndoButton(bool isHover)
        {
            UndoButton.EnableClass(isHover, "UndoButton".EHover());
            UndoIcon.EnableClass(isHover, "UndoIcon".EHover());
        }
    }
}
#endif