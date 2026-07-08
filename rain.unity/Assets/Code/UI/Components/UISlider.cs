using System;
using Code.UI.Components.Selected;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Components
{
    public class UISlider : AUISelectable
    {
        public event Action<float> Changed;

        public float Value => _slider == null ? 0 : _slider.value;
        
        [SerializeField] private Slider _slider;

        
        public void Subscribe()
        {
            _slider.onValueChanged.AddListener(value => Changed?.Invoke(value));
        }
        
        public void Unsubscribe()
        {
            _slider.onValueChanged.RemoveAllListeners();
        }

        public override void SetInteractable(bool isInteractable)
        {
            _slider.enabled = isInteractable;
        }

        public void SetValueWithoutNotify(float value)
        {
            _slider.SetValueWithoutNotify(value);
        }
        
        public void SetValue(float value)
        {
            _slider.value = value;
        }
    }
}