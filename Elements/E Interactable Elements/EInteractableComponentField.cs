#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editor.Extension;

namespace YNL.Editor.UIElement
{
    public class EInteractableComponentField<T> : Button where T : UnityEngine.Object
    {
        public Action OnPointerDown;
        public Action OnPointerUp;
        public Action OnPointerEnter;
        public Action OnPointerExit;
        public Action OnPointerMove;

        public Action OnDragEnter;
        public Action OnDragExit;
        public Action OnDragUpdate;
        public Action<T> OnDragPerform;

        public T ReferencedObject;

        public EInteractableComponentField() : base()
        {
            RegisterCallback<PointerDownEvent>(evt => OnEventPointerDown(evt, OnPointerDown), TrickleDown.TrickleDown);
            RegisterCallback<PointerUpEvent>(evt => OnEventPointerUp(evt, OnPointerUp));
            RegisterCallback<PointerEnterEvent>(evt => OnEventPointerEnter(evt, OnPointerEnter));
            RegisterCallback<PointerLeaveEvent>(evt => OnEventPointerLeave(evt, OnPointerExit));
            RegisterCallback<PointerMoveEvent>(evt => OnEventPointerMove(evt, OnPointerMove));

            RegisterCallback<DragEnterEvent>(evt => OnEventDragEnter(evt, OnDragEnter));
            RegisterCallback<DragLeaveEvent>(evt => OnEventDragLeave(evt, OnDragExit));
            RegisterCallback<DragUpdatedEvent>(evt => OnEventDragUpdated(evt, OnDragUpdate));
            RegisterCallback<DragPerformEvent>(evt => OnEventDragPerform(evt, OnDragPerform));
        }

        public static void OnEventPointerEnter(PointerEnterEvent evt, Action action)
        {
            var element = evt.currentTarget as EInteractableComponentField<T>;
            action?.Invoke();
            element.PointerEnter();
            evt.StopPropagation();
        }
        public static void OnEventPointerLeave(PointerLeaveEvent evt, Action action)
        {
            var element = evt.currentTarget as EInteractableComponentField<T>;
            action?.Invoke();
            element.PointerExit();
            evt.StopPropagation();
        }
        public static void OnEventPointerDown(PointerDownEvent evt, Action action)
        {
            var element = evt.currentTarget as EInteractableComponentField<T>;
            action?.Invoke();
            element.PointerDown();
            evt.StopPropagation();
        }
        public static void OnEventPointerUp(PointerUpEvent evt, Action action)
        {
            var element = evt.currentTarget as EInteractableComponentField<T>;
            action?.Invoke();
            element.PointerUp();
            evt.StopPropagation();
        }
        public static void OnEventPointerMove(PointerMoveEvent evt, Action action)
        {
            var element = evt.currentTarget as EInteractableComponentField<T>;
            action?.Invoke();
            element.PointerMove();
            evt.StopPropagation();
        }

        public static void OnEventDragEnter(DragEnterEvent evt, Action action)
        {
            var element = evt.currentTarget as EInteractableComponentField<T>;

            if (DragAndDrop.objectReferences.TryGet(0) is GameObject)
            {
                T component = ((GameObject)DragAndDrop.objectReferences.TryGet(0)).GetComponent<T>();

                if (!component.IsNull() && component is T)
                {
                    element.DragEnter();
                    action?.Invoke();
                }

            }

            evt.StopPropagation();
        }
        public static void OnEventDragLeave(DragLeaveEvent evt, Action action)
        {
            var element = evt.currentTarget as EInteractableComponentField<T>;

            if (DragAndDrop.objectReferences.TryGet(0) is GameObject)
            {
                T component = ((GameObject)DragAndDrop.objectReferences.TryGet(0)).GetComponent<T>();

                if (!component.IsNull() && component is T)
                {
                    element.DragLeave();
                    action?.Invoke();
                }
            }

            evt.StopPropagation();
        }
        public static void OnEventDragUpdated(DragUpdatedEvent evt, Action action)
        {
            var element = evt.currentTarget as EInteractableComponentField<T>;

            if (DragAndDrop.objectReferences.TryGet(0) is GameObject)
            {
                T component = ((GameObject)DragAndDrop.objectReferences.TryGet(0)).GetComponent<T>();

                if (!component.IsNull() && component is T)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                    element.DragUpdate();
                    action?.Invoke();
                }
            }

            evt.StopPropagation();
        }
        public static void OnEventDragPerform(DragPerformEvent evt, Action<T> action)
        {
            var element = evt.currentTarget as EInteractableComponentField<T>;

            if (DragAndDrop.objectReferences.TryGet(0) is GameObject)
            {
                T component = ((GameObject)DragAndDrop.objectReferences.TryGet(0)).GetComponent<T>();

                if (!component.IsNull() && component is T)
                {
                    element.ReferencedObject = ((GameObject)DragAndDrop.objectReferences.TryGet(0)).GetComponent<T>();
                    element.DragUp();
                    action?.Invoke(element.ReferencedObject);
                }
            }

            evt.StopPropagation();
        }

        public virtual void PointerDown() { }
        public virtual void PointerUp() { }
        public virtual void PointerEnter() { }
        public virtual void PointerExit() { }
        public virtual void PointerMove() { }

        public virtual void DragEnter() { }
        public virtual void DragLeave() { }
        public virtual void DragUpdate() { }
        public virtual void DragUp() { }
    }
}
#endif