#if UNITY_EDITOR
using System;
using UnityEngine.UIElements;

namespace YNL.Editor.UIElement
{
    public class EInteractableImage : Image
    {
        public Action OnPointerDown;
        public Action OnPointerUp;
        public Action OnPointerEnter;
        public Action OnPointerExit;

        public EInteractableImage() : base()
        {
            RegisterCallback<PointerDownEvent>(evt => OnEventPointerDown(evt, OnPointerDown));
            RegisterCallback<PointerUpEvent>(evt => OnEventPointerUp(evt, OnPointerUp));
            RegisterCallback<PointerEnterEvent>(evt => OnEventPointerEnter(evt, OnPointerEnter));
            RegisterCallback<PointerLeaveEvent>(evt => OnEventPointerLeave(evt, OnPointerExit));
        }

        public static void OnEventPointerEnter(PointerEnterEvent evt, Action action)
        {
            var image = evt.currentTarget as EInteractableImage;
            action?.Invoke();
            image.PointerEnter();
            evt.StopPropagation();
        }
        public static void OnEventPointerLeave(PointerLeaveEvent evt, Action action)
        {
            var image = evt.currentTarget as EInteractableImage;
            action?.Invoke();
            image.PointerExit();
            evt.StopPropagation();
        }
        public static void OnEventPointerDown(PointerDownEvent evt, Action action)
        {
            var image = evt.currentTarget as EInteractableImage;
            action?.Invoke();
            image.PointerDown();
            evt.StopPropagation();
        }
        public static void OnEventPointerUp(PointerUpEvent evt, Action action)
        {
            var image = evt.currentTarget as EInteractableImage;
            action?.Invoke();
            image.PointerUp();
            evt.StopPropagation();
        }

        public virtual void PointerDown() { }
        public virtual void PointerUp() { }
        public virtual void PointerEnter() { }
        public virtual void PointerExit() { }
    }
}
#endif