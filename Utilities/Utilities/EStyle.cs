using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editor.Extensions;

namespace YNL.Editor.Utilities
{
    public static class EStyle
    {
        #region ▶ Element Style - Containing

        /// <summary>
        /// Add elements to a container element.
        /// </summary>
        public static T AddElements<T>(this T container, params VisualElement[] elements) where T : VisualElement
        {
            foreach (var element in elements) container.Add(element); 
            return container;
        }

        /// <summary>
        /// Remove all child elements from a container element.
        /// </summary>
        public static T RemoveAllElements<T>(this T container) where T : VisualElement
        {
            foreach (var element in container.Children().ToList()) element.RemoveFromHierarchy();
            return container;
        }

        #endregion

        #region ▶ Element Style - Class

        /// <summary>
        /// Add classes to a visual element.
        /// </summary>
        public static T AddClass<T>(this T element, params string[] classes) where T : VisualElement
        {
            foreach (var className in classes)
            {
                if (!element.ClassListContains(className)) element.AddToClassList(className);
            }
            return element;
        }

        /// <summary>
        /// Add classes to a visual element.
        /// </summary>
        public static T EnableClass<T>(this T element, bool enable, params string[] classes) where T : VisualElement
        {
            foreach (var className in classes) element.EnableInClassList(className, enable);
            return element;
        }
        public static T EnableClass<T>(this T element, params string[] classes) where T : VisualElement
            => element.EnableClass(true, classes);
        public static T DisableClass<T>(this T element, params string[] classes) where T : VisualElement
            => element.EnableClass(false, classes);

        /// <summary>
        /// Remove classes from a visual element.
        /// </summary>
        public static T RemoveClass<T>(this T element, params string[] classes) where T : VisualElement
        {
            foreach (var className in classes)
            {
                if (element.ClassListContains(className)) element.RemoveFromClassList(className);
            }
            return element;
        }

        #endregion

        #region ▶ Element Style - Style Sheet

        public static T AddStyle<T>(this T element, params StyleSheet[] styles) where T : VisualElement
        {
            foreach (var style in styles)
            {
                if (!element.styleSheets.Contains(style)) element.styleSheets.Add(style);
            }
            return element;
        }

        public static T AddStyle<T>(this T element, params string[] stylePaths) where T : VisualElement
        {
#if UNITY_EDITOR
            foreach (var stylePath in stylePaths) element.AddStyle(Resources.Load<StyleSheet>(stylePath));
#endif
            return element;
        }

        public static T RemoveStyle<T>(this T element, params StyleSheet[] styles) where T : VisualElement
        {
            foreach (var style in styles)
            {
                if (element.styleSheets.Contains(style)) element.styleSheets.Remove(style);
            }
            return element;
        }

        public static T RemoveStyle<T>(this T element, params string[] stylePaths) where T : VisualElement
        {
#if UNITY_EDITOR
            foreach (var stylePath in stylePaths)
            {
                StyleSheet style = Resources.Load<StyleSheet>(stylePath);
                for (int i = 0; i < element.styleSheets.count; i++)
                {
                    if (element.styleSheets[i].name == style.name) element.styleSheets.Remove(element.styleSheets[i]);
                }
            }
#endif
            return element;
        }

        #endregion

        #region ▶ Element Style - Name

        /// <summary>
        /// Set name of a visual element.
        /// </summary>
        public static T SetName<T>(this T element, string nane) where T : VisualElement
        {
            element.name = nane;
            return element;
        }

        /// <summary>
        /// Get name of a visual element.
        /// </summary>
        public static string GetName<T>(this T element) where T : VisualElement
            => element.name;

        #endregion

        #region X Element Style - Tooltip

        #endregion

        #region ▶ Element Style - Space

        public static T AddSpace<T>(this T target, float width, float height) where T : VisualElement =>
            target.AddElements(new VisualElement().SetName("Space").SetSize(width, height));

        public static T AddHorizontalSpace<T>(this T target, float height) where T : VisualElement =>
            target.AddElements(new VisualElement().SetName("HSpace").SetHeight(height));

        public static T AddVerticalSpace<T>(this T target, float width) where T : VisualElement =>
            target.AddElements(new VisualElement().SetName("VSpace").SetWidth(width));

        #endregion

        #region X Element Style - Picking Mode

        #endregion

        #region X Element Style - Position
        public static T SetTop<T>(this T target, float value) where T : VisualElement
        {
            target.style.top = value;
            return target;
        }
        #endregion

        #region X Element Style - Overflow

        #endregion

        #region ▶ Element Style - Align Self

        /// <summary>
        /// Align this element itself.
        /// </summary>
        public static T SetAlignSelf<T>(this T target, Align value) where T : VisualElement
        {
            target.style.alignSelf = new StyleEnum<Align>(value);
            return target;
        }

        /// <summary>
        /// Get align-self value of this element.
        /// <para/> style.alignSelf
        public static Align GetAlignSelf<T>(this T target) where T : VisualElement =>
            target.style.alignSelf.value;

        #endregion

        #region X Element Style - Align Items

        #endregion

        #region X Element Style - Align Content

        #endregion

        #region X Element Style - Justify Content

        #endregion

        #region X Element Style - Flex Direction

        #endregion

        #region X Element Style - Flex Grow

        #endregion

        #region X Element Style - Flex Shrink

        #endregion

        #region X Element Style - Flex Wrap

        #endregion

        #region X Element Style - Flex Basis

        #endregion

        #region X Element Style - Border Color

        #endregion

        #region X Element Style - Border Width

        #endregion

        #region X Element Style - Border Radius

        #endregion

        #region X Element Style - Background Color

        public static T SetBackgroundColor<T>(this T element, string hex) where T : VisualElement
            => element.SetBackgroundColor(EEditorExtension.ToColor(hex));
        public static T SetBackgroundColor<T>(this T element, Color color) where T : VisualElement
        {
            element.style.backgroundColor = color;
            return element;
        }

        public static Color GetBackgroundColor<T>(this T element) where T : VisualElement
            => element.resolvedStyle.backgroundColor;

        #endregion

        #region X Element Style - Opacity

        #endregion

        #region ▶ Element Style - Height

        #region ▶ Element Style - Height
        public static T SetHeight<T>(this T element, float height, bool percent = false) where T : VisualElement
        {
            if (percent) element.style.height = new Length(height, LengthUnit.Percent);
            else element.style.height = height;
            return element;
        }
        public static T SetHeight<T>(this T target, StyleKeyword styleKeyword) where T : VisualElement
        {
            target.style.height = new StyleLength(styleKeyword);
            return target;
        }
        public static T ResetHeight<T>(this T target) where T : VisualElement
            => target.SetHeight(StyleKeyword.Auto);
        public static float GetHeight<T>(this T target) where T : VisualElement
            => target.style.height.value.value;
        #endregion
        #region ▶ Element Style - Min Height
        public static T SetMinHeight<T>(this T element, int Height) where T : VisualElement
        {
            element.style.minHeight = Height;
            return element;
        }
        public static T SetMinHeight<T>(this T target, StyleKeyword styleKeyword) where T : VisualElement
        {
            target.style.minHeight = new StyleLength(styleKeyword);
            return target;
        }
        public static T ResetMinHeight<T>(this T target) where T : VisualElement
            => target.SetHeight(StyleKeyword.Auto);
        public static float GetMinHeight<T>(this T target) where T : VisualElement
            => target.style.minHeight.value.value;
        #endregion
        #region ▶ Element Style - Max Height
        public static T SetMaxHeight<T>(this T element, int Height) where T : VisualElement
        {
            element.style.maxHeight = Height;
            return element;
        }
        public static T SetMaxHeight<T>(this T target, StyleKeyword styleKeyword) where T : VisualElement
        {
            target.style.maxHeight = new StyleLength(styleKeyword);
            return target;
        }
        public static T ResetMaxHeight<T>(this T target) where T : VisualElement
            => target.SetHeight(StyleKeyword.Auto);
        public static float GetMaxHeight<T>(this T target) where T : VisualElement
            => target.style.maxHeight.value.value;
        #endregion

        #endregion

        #region ▶ Element Style - Width

        #region ▶ Element Style - Width
        public static T SetWidth<T>(this T element, float width, bool percent = false) where T : VisualElement
        {
            if (percent) element.style.width = new Length(width, LengthUnit.Percent);
            else element.style.width = width;
            return element;
        }
        public static T SetWidth<T>(this T target, StyleKeyword styleKeyword) where T : VisualElement
        {
            target.style.width = new StyleLength(styleKeyword);
            return target;
        }

        public static T ResetWidth<T>(this T target) where T : VisualElement 
            => target.SetWidth(StyleKeyword.Auto);
        public static float GetWidth<T>(this T target) where T : VisualElement 
            => target.style.width.value.value;
        #endregion
        #region ▶ Element Style - Min Width
        public static T SetMinWidth<T>(this T element, float width, bool percent = false) where T : VisualElement
        {
            if (!percent) element.style.minWidth = new Length(width, LengthUnit.Percent);
            else element.style.minWidth = width;
            return element;
        }
        public static T SetMinWidth<T>(this T target, StyleKeyword styleKeyword) where T : VisualElement
        {
            target.style.minWidth = new StyleLength(styleKeyword);
            return target;
        }
        public static T ResetMinWidth<T>(this T target) where T : VisualElement
            => target.SetWidth(StyleKeyword.Auto);
        public static float GetMinWidth<T>(this T target) where T : VisualElement
            => target.style.minWidth.value.value;
        #endregion
        #region ▶ Element Style - Max Width
        public static T SetMaxWidth<T>(this T element, float width, bool percent = false) where T : VisualElement
        {
            if (!percent) element.style.maxWidth = new Length(width, LengthUnit.Percent);
            else element.style.maxWidth = width;
            return element;
        }
        public static T SetMaxWidth<T>(this T target, StyleKeyword styleKeyword) where T : VisualElement
        {
            target.style.maxWidth = new StyleLength(styleKeyword);
            return target;
        }
        public static T ResetMaxWidth<T>(this T target) where T : VisualElement
            => target.SetWidth(StyleKeyword.Auto);
        public static float GetMaxWidth<T>(this T target) where T : VisualElement
            => target.style.maxWidth.value.value;
        #endregion

        #endregion

        #region ▶ Element Style - Size

        /// <summary>
        /// Set the fixed values for the width and height of an element for the layout
        /// </summary>
        public static T SetSize<T>(this T target, float width, float height) where T : VisualElement =>
            target.SetWidth(width).SetHeight(height);

        /// <summary>
        /// Set the same fixed value for the width and height of an element for the layout
        /// </summary>
        public static T SetSize<T>(this T target, float value) where T : VisualElement =>
            target.SetSize(value, value);

        /// <summary>
        /// Set the same fixed value for the width and height of an element for the layout
        /// </summary>
        public static T SetSize<T>(this T target, StyleKeyword styleKeyword) where T : VisualElement
        {
            target.SetWidth(styleKeyword).SetHeight(styleKeyword);
            return target;
        }

        /// <summary>
        /// Set the width and height of an element to auto
        /// </summary>
        public static T ResetSize<T>(this T target) where T : VisualElement =>
            target.SetSize(StyleKeyword.Auto);

        #endregion

        #region ▶ Element Style - Background Image

        /// <summary> 
        /// Set the background image to paint in the element's box 
        /// </summary>
        public static T SetBackgroundImage<T>(this T target, Texture2D value) where T : VisualElement
        {
            target.style.backgroundImage = new StyleBackground(value);
            return target;
        }
#if UNITY_EDITOR
        public static T SetBackgroundImage<T>(this T target, string path) where T : VisualElement
        {
            target.style.backgroundImage = new StyleBackground(Resources.Load<Texture2D>(path));
            return target;
        }
#endif

        /// <summary> 
        /// Get the background image Texture2D used to paint in the element's box 
        /// </summary>
        public static Texture2D GetBackgroundImage<T>(this T target) where T : VisualElement 
            => target.style.backgroundImage.value.texture;

        #endregion

        #region X Element Style - Unity Background Image Tint Color

        public static T SetBackgroundImageTintColor<T>(this T element, Color color) where T : VisualElement
        {
            element.style.unityBackgroundImageTintColor = color;
            return element;
        }

        public static Color GetBackgroundImageTintColor<T>(this T element) where T : VisualElement
        {
            return element.resolvedStyle.unityBackgroundImageTintColor;
        }
        #endregion

        #region X Element Style - Unity Slice

        #endregion

        #region ▶ Element Style - Color

        /// <summary>
        /// Set the color to use when drawing the text of an element
        /// </summary>
        public static T SetColor<T>(this T element, Color value) where T : VisualElement
        {
            element.style.color = value;
            return element;
        }
        public static T SetColor<T>(this T element, string hex) where T : VisualElement
            => element.SetColor(EEditorExtension.ToColor(hex));

        /// <summary>
        /// Get the color used when drawing the text of an element
        /// </summary>
        public static Color GetColor<T>(this T element) where T : VisualElement
            => element.resolvedStyle.color;

        #endregion

        #region X Element Style - White Space

        #endregion

        #region X Element Style - Unity Font

        #endregion

        #region X Element Style - Font Size

        #endregion

        #region X Element Style - Unity Text Align

        #endregion

        #region ▶ Element Style - Margin

        public static T SetMarginLeft<T>(this T element, float value) where T : VisualElement
        {
            element.style.marginLeft = value;
            return element;
        }
        public static float GetMarginLeft<T>(this T element) where T : VisualElement
            => element.resolvedStyle.marginLeft;
        public static T SetMarginTop<T>(this T element, float value) where T : VisualElement
        {
            element.style.marginTop = value;
            return element;
        }
        public static float GetMarginTop<T>(this T element) where T : VisualElement
            => element.resolvedStyle.marginTop;
        public static T SetMarginRight<T>(this T element, float value) where T : VisualElement
        {
            element.style.marginRight = value;
            return element;
        }
        public static float GetMarginRight<T>(this T element) where T : VisualElement
            => element.resolvedStyle.marginRight;
        public static T SetMarginBottom<T>(this T element, float value) where T : VisualElement
        {
            element.style.marginBottom = value;
            return element;
        }
        public static float GetMarginBottom<T>(this T element) where T : VisualElement
            => element.resolvedStyle.marginBottom;

        /// <summary> 
        /// Set all margins to 0 (zero) 
        /// </summary>
        public static T ClearMargin<T>(this T element) where T : VisualElement 
            => element.SetMargin(0);

        /// <summary>
        /// Set the space reserved for the all the edges margins during the layout phase (Left, Top, Right, Bottom)
        /// </summary>
        public static T SetMargin<T>(this T element, float left, float top, float right, float bottom) where T : VisualElement
        {
            element.SetMarginLeft(left).SetMarginTop(top).SetMarginRight(right).SetMarginBottom(bottom);
            return element;
        }

        /// <summary>
        /// Set the same space reserved for the all the edges margins during the layout phase (Left, Top, Right, Bottom).
        /// </summary>
        public static T SetMargin<T>(this T element, float value) where T : VisualElement =>
            element.SetMargin(value, value, value, value);

        #endregion

        #region X Element Style - Padding

        #endregion

        #region X Element Style - Resize

        #endregion

        #region ▶ Element Style - Transition

        public static T SetTransitionProperty<T>(this T element, params string[] properties) where T : VisualElement
        {
            List<StylePropertyName> propertyNames = new();
            foreach (var property in properties) propertyNames.Add(property);
            element.style.transitionProperty = propertyNames;
            return element;
        }

        public static T SetTransitionDuration<T>(this T element, params float[] values) where T : VisualElement
        {
            List<TimeValue> timeValues = new();
            foreach (var value in values) timeValues.Add(new TimeValue(value, TimeUnit.Second));
            element.style.transitionDuration = timeValues;
            return element;
        }

        #endregion

        #region ▶ Text Element Style - Text

        public static T SetText<T>(this T element, string text) where T : TextElement
        {
            element.text = text;
            return element;
        }

        public static string GetText<T>(this T element, string text) where T : TextElement
            => element.text;

        #endregion
    }

    public static class EProperty
    {
        public static T SetText<T>(this T element, string text) where T : TextField
        {
            element.value = text;
            return element;
        }
    }
}