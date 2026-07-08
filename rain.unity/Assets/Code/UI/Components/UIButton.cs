using Code.UI.Components.Selected;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Components
{
    public class UIButton : AUISelectable
    {
        [SerializeField] private Image _image;
        
        public override void SetInteractable(bool isInteractable)
        {
            _image.raycastTarget = isInteractable;
        }
    }
}