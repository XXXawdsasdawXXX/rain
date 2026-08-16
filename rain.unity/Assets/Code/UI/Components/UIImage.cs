using DG.Tweening;
using Essential;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Code.UI.Components
{
    public class UIImage : AUIComponent
    {
        [field: SerializeField] public Image Render { get; private set; }

        [SerializeField] private Vector2 _fullSize; 
        [SerializeField] private Vector2 _emptySize;

        private Tween _tween;

        
        public void SetFill(float normalizeValue)
        {
            _tween?.Kill();
            _tween = Render.rectTransform.DOSizeDelta(Vector2.Lerp(_emptySize, _fullSize, normalizeValue), 0.3f);
        }

        [Button("test - SetFillWithoutAnimation")]
        public void SetFillWithoutAnimation(float normalizeValue)
        {
            Render.rectTransform.sizeDelta = Vector2.Lerp(_emptySize, _fullSize, normalizeValue);
        }

#if UNITY_EDITOR
        
        [Button("editor - _setCurrentSizeAsFullSize")]
        private void _setCurrentSizeAsFullSize()
        {
            if (Render == null)
            {
                Log.Error(this, "[_setCurrentSizeAsFullSize] component has not render");
                return;
            }

            _fullSize = Render.rectTransform.sizeDelta;
            _emptySize = _fullSize.x > _fullSize.y ? new Vector2(0, _fullSize.y) : new Vector2(_fullSize.x, 0);
            
            EditorUtility.SetDirty(this);
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (Render == null)
            {
                if(TryGetComponent(out Image image))
                {
                    Render = image;
                }
            }
        }

#endif
    }
}