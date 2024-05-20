#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine.UIElements;
using YNL.Editors.Windows.Utilities;

namespace YNL.Editors.UIElements
{
    public class EInteractableAssetsField<T> : Button where T : UnityEngine.Object
    {
        public Action OnPointerDown;
        public Action OnPointerUp;
        public Action OnPointerEnter;
        public Action OnPointerExit;
        public Action OnPointerMove;

        public Action OnDragEnter;
        public Action OnDragExit;
        public Action OnDragUpdate;
        public Action<T[]> OnDragPerform;

        public T[] BindedObjects;

        public EInteractableAssetsField() : base()
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
            var element = evt.currentTarget as EInteractableAssetsField<T>;
            action?.Invoke();
            element.PointerEnter();
            evt.StopPropagation();
        }
        public static void OnEventPointerLeave(PointerLeaveEvent evt, Action action)
        {
            var element = evt.currentTarget as EInteractableAssetsField<T>;
            action?.Invoke();
            element.PointerExit();
            evt.StopPropagation();
        }
        public static void OnEventPointerDown(PointerDownEvent evt, Action action)
        {
            var element = evt.currentTarget as EInteractableAssetsField<T>;
            action?.Invoke();
            element.PointerDown();
            evt.StopPropagation();
        }
        public static void OnEventPointerUp(PointerUpEvent evt, Action action)
        {
            var element = evt.currentTarget as EInteractableAssetsField<T>;
            action?.Invoke();
            element.PointerUp();
            evt.StopPropagation();
        }
        public static void OnEventPointerMove(PointerMoveEvent evt, Action action)
        {
            var element = evt.currentTarget as EInteractableAssetsField<T>;
            action?.Invoke();
            element.PointerMove();
            evt.StopPropagation();
        }

        public static void OnEventDragEnter(DragEnterEvent evt, Action action)
        {
            var element = evt.currentTarget as EInteractableAssetsField<T>;

            if (DragAndDrop.objectReferences.ETryGet(0) is T)
            {
                element.DragEnter();
                action?.Invoke();
            }

            evt.StopPropagation();
        }
        public static void OnEventDragLeave(DragLeaveEvent evt, Action action)
        {
            var element = evt.currentTarget as EInteractableAssetsField<T>;

            if (DragAndDrop.objectReferences.ETryGet(0) is T)
            {
                element.DragLeave();
                action?.Invoke();
            }

            evt.StopPropagation();
        }
        public static void OnEventDragUpdated(DragUpdatedEvent evt, Action action)
        {
            var element = evt.currentTarget as EInteractableAssetsField<T>;

            if (DragAndDrop.objectReferences.ETryGet(0) is T)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                element.DragUpdate();
                action?.Invoke();
            }

            evt.StopPropagation();
        }
        public static void OnEventDragPerform(DragPerformEvent evt, Action<T[]> action)
        {
            var element = evt.currentTarget as EInteractableAssetsField<T>;

            element.BindedObjects = DragAndDrop.objectReferences as T[];
            element.DragUp();
            action?.Invoke(element.BindedObjects);

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