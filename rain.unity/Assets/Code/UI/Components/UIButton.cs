using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Components
{
    public class UIButton : AUIComponent
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _disableColor = new(0.4f, 0.4f, 0.5f, 1);

        private Tween _tween; 
        
        
        public override void SetInteractable(bool isInteractable)
        {
            if (_image.raycastTarget == isInteractable)
            {
                return;
            }
            
            _image.raycastTarget = isInteractable;
            
            _tween?.Kill();

            _tween = _image.DOColor(isInteractable ? _defaultColor : _disableColor, 0.3f)
                .SetLink(gameObject, LinkBehaviour.KillOnDisable);
            
            base.SetInteractable(isInteractable);
        }
    }
}