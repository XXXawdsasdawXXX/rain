using System;
using Code.UI.Components.Selected;
using TMPro;
using UnityEngine;

namespace Code.UI.Components
{
    public class UIInputField : AUISelectable
    {
        public event Action<string> Changed;
   
        public string Value => _inputField.text;
        
        [SerializeField] private TMP_InputField _inputField;

        
        private void Awake()
        {
            _inputField.onEndEdit.AddListener(_endEdit);
        }

        public void SetTextWithoutNotify(string text)
        {
            _inputField.SetTextWithoutNotify(text);
        }

        public override void SetInteractable(bool isInteractable)
        {
            _inputField.interactable = isInteractable;
        }

        private void _endEdit(string value)
        {
            Changed?.Invoke(value);
        }
    }
}