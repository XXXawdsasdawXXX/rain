using System;
using JetBrains.Annotations;
using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.UI.Components.Selected
{
    public abstract class AUISelectable : Essential.Mono, 
        IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        public event Action Selected;
        public event Action Deselected;
        public event Action Clicked;

        
        [field: Group("Selectable"), GUIColor(0.1f, 0.85f, 0.55f, 0.9f)]
        [field: SerializeField] protected GameObject body { get; private set; }
        [field: Group("Selectable"), GUIColor(0.1f, 0.85f, 0.55f, 0.9f)]
        [field: SerializeField] public bool AutoSelect { get; private set; }
      
        [field: Group("Selectable"), GUIColor(0.1f, 0.85f, 0.55f, 0.9f)]
        [field: SerializeField] public bool IsSelected { get; private set; }
        
        [Group("Selectable")] 
        [SerializeField, GUIColor(0.1f, 0.85f, 0.55f, 0.9f)]
        private bool _hasAnimation;
        
        [field: Group("Selectable"), GUIColor(0.1f, 0.85f, 0.55f, 0.9f), ShowIf(nameof(_hasAnimation))]
        [SerializeField, CanBeNull] private UISelectableAnimation _selectableAnimation;
        
        
        
        public void Dispose()
        {
            Selected = null;
            Deselected = null;
            Clicked = null;
        }

        public virtual void Select()
        {
            if (_selectableAnimation != null)
            {
                _selectableAnimation.Select();
            }
            
            IsSelected = true;
        }

        public virtual void Deselect()
        {
            if (_selectableAnimation != null)
            {
                _selectableAnimation.Deselect();
            }
            
            IsSelected = false;
        }
        
        public abstract void SetInteractable(bool isInteractable);
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            onEnter();

            if (AutoSelect)
            {
                Select();
            }
            
            Selected?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            onClick();
            Clicked?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onExit();
         
            if (AutoSelect)
            {
                Deselect();
            }
            
            Deselected?.Invoke();
        }

        protected virtual void onEnter()
        {
        }

        protected virtual void onClick()
        {
        }

        protected virtual void onExit()
        {
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (body == null)
            {
                body = gameObject;
            }
        }
#endif
    }
}