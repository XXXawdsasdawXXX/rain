using System;
using System.Collections.Generic;
using System.Linq;
using Code.UI.Components.Selected;
using TMPro;
using UnityEngine;

namespace Code.UI.Components
{
    public class UIDropDown : AUISelectable
    {
        public event Action<int> Changed;
        
        [field: SerializeField] public TMP_Dropdown DropDown { get; private set; }

        
        private void OnEnable()
        {
            DropDown.onValueChanged.AddListener(_onChanged);
        }

        private void OnDisable()
        {
            DropDown.onValueChanged.RemoveListener(_onChanged);
        }

        public void SetOptions(List<string> values)
        {
            DropDown.ClearOptions();

            List<TMP_Dropdown.OptionData> options = values.Select(v => new TMP_Dropdown.OptionData(v)).ToList();

            DropDown.AddOptions(options);
        }
        
        public void SetOptions<T>(T[] values)
        {
            DropDown.ClearOptions();

            List<TMP_Dropdown.OptionData> options = values
                .Select(v => new TMP_Dropdown.OptionData(v.ToString()))
                .ToList();

            DropDown.AddOptions(options);
        }

        public void SetOptions<T>(List<T> values)
        {
            DropDown.ClearOptions();

            List<TMP_Dropdown.OptionData> options = values
                .Select(v => new TMP_Dropdown.OptionData(v.ToString()))
                .ToList();

            DropDown.AddOptions(options);
        }

        public void SetCurrent(int id)
        {
            DropDown.value = id;
        }

        public void SetCurrentWithoutNotify(int id)
        {
            DropDown.SetValueWithoutNotify(id);
        }

        public override void SetInteractable(bool isInteractable)
        {
            DropDown.interactable = isInteractable;
            
            base.SetInteractable(isInteractable);
        }

        private void _onChanged(int id)
        {
            Changed?.Invoke(id);
        }
    }
}