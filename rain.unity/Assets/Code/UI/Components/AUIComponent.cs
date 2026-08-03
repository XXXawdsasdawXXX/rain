using System;
using JetBrains.Annotations;
using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.UI.Components
{
    public abstract class AUIComponent : Essential.Mono, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        public event Action Hover;
        public event Action UnHover;
        public event Action Clicked;
        public bool IsInteractable { get; private set; }

        [field: Group("Selectable"), GUIColor(0.1f, 0.85f, 0.55f, 0.9f)]
        [field: SerializeField] protected GameObject body { get; private set; }
        [field: Group("Selectable"), GUIColor(0.1f, 0.85f, 0.55f, 0.9f)]
        [field: SerializeField] public bool AutoSelect { get; private set; }
      
        [field: Group("Selectable"), GUIColor(0.1f, 0.85f, 0.55f, 0.9f)]
        [field: SerializeField] public bool IsHovered { get; private set; }
        
        [Group("Selectable")] 
        [SerializeField, GUIColor(0.1f, 0.85f, 0.55f, 0.9f)]
        private bool _hasAnimation;
        
        [field: Group("Selectable"), GUIColor(0.1f, 0.85f, 0.55f, 0.9f), ShowIf(nameof(_hasAnimation))]
        [SerializeField, CanBeNull] private AUIHoverAnimation _hoverAnimation;
        
        
        public void Dispose()
        {
            Hover = null;
            UnHover = null;
            Clicked = null;
        }

        public virtual void Select()
        {
            if (_hoverAnimation != null)
            {
                _hoverAnimation.InvokeHoverAnimation();
            }
            
            IsHovered = true;
        }

        public virtual void Deselect()
        {
            if (_hoverAnimation != null)
            {
                _hoverAnimation.InvokeUnHoverAnimation();
            }
            
            IsHovered = false;
        }

        public virtual void SetInteractable(bool isInteractable)
        {
            IsInteractable = isInteractable;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            onHover();

            if (AutoSelect)
            {
                Select();
            }
            
            Hover?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            onClick();
            Clicked?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onUnHover();
         
            if (AutoSelect)
            {
                Deselect();
            }
            
            UnHover?.Invoke();
        }

        protected virtual void onHover()
        {
        }

        protected virtual void onClick()
        {
        }

        protected virtual void onUnHover()
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