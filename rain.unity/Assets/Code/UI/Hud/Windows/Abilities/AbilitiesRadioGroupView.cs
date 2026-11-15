using System;
using System.Collections.Generic;
using Code.Game.World.Abilities;
using Code.Game.World.Characters;
using Code.UI.Components.Window;
using Essential;
using UnityEngine;

namespace Code.UI.Hud.Windows.Abilities
{
    public class AbilitiesRadioGroupView : AUIWindowView
    {
        public event Action<AbilityModel> Choiced; 

        [SerializeField] private MonoPool<AbilityButton> _pool;

        private AbilityButton _current;
        
        
        
        public void SetHero(CharacterActor owner, IReadOnlyList<AbilityModel> abilityModels)
        {
            _pool.DisableAll();

            if (owner == null || abilityModels == null || abilityModels.Count == 0)
            {
                return;
            }
            
            Debug.Log($"set abilities models {abilityModels.Count}");
            foreach (AbilityModel model in abilityModels)
            {
                AbilityButton button = _pool.GetNext();
                button.SetAbilityModel(model);
                button.SetInteractable(AbilityResolver.CanUse(owner, model));
                button.Clicked += () =>
                {
                    if (_current != null)
                    {
                        _current.UnCheck();
                    }

                    _current = button;
                    
                    _current.Check();
                    
                    Choiced?.Invoke(button.Model);
                };
            }
        }
        
    }
    

}