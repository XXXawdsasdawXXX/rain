using Code.Game.World.Abilities;
using Code.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Hud.Windows.Abilities
{
    public class AbilityButton : UIRadioButton
    {
        
        [SerializeField] private Image _icon;

        public AbilityModel Model { get; private set; }
        
        
        public void SetAbilityModel(AbilityModel model)
        {
            Model = model;
            _icon.sprite = model.Icon;
        }
    }
}