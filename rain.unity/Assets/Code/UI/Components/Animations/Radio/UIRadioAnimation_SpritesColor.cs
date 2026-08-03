using System;
using DG.Tweening;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Components.Animations
{
    [Serializable]
    public class UIRadioAnimation_SpritesColor : AUIRadioAnimation
    {
        [Serializable]
        private class Model
        {
            public Image Render;
            public Color CheckColor = Color.white;
            public Color UnCheckColor = Color.white;
        }

        private const float BLEND_DURATION = 0.5f;
        
        [SerializeField] private Model[] Models;

        private Sequence _sequence;
        
        
        public override void InvokeCheckAnimation()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            foreach (Model model in Models)
            {
                _sequence.Append(model.Render.DOColor(model.CheckColor, BLEND_DURATION));
            }

            _sequence.Play();
        }

        public override void InvokeUnCheckAnimation()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            foreach (Model model in Models)
            {
                _sequence.Append(model.Render.DOColor(model.UnCheckColor, BLEND_DURATION));
            }

            _sequence.Play();
        }
        
#if UNITY_EDITOR

        [Button]
        private void _setCommonColors(Color checkColor, Color unCheckColor)
        {
            foreach (Model model in Models)
            {
                model.CheckColor = checkColor;
                model.UnCheckColor = unCheckColor;
            }
        }
#endif
    }
}