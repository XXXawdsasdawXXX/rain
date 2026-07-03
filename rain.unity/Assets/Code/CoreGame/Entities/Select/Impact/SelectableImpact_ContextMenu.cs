using System;
using Essential;
using UnityEngine;

namespace Code.CoreGame.Entities.Select.Impact
{
    [Serializable]
    public class SelectableImpact_ContextMenu : SelectableImpact
    {
        [SerializeField] private RectTransform _menu;
        
        public override void Hovered(bool isHover)
        {
            _menu.gameObject.SetActive(isHover);
        }

        public override void Pressed()
        {
         
        }

        public override bool CanUnHover()
        {
            return !UIUtils.IsCursorOverUIElement(_menu, Camera.main);
        }
    }
}