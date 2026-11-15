using Code.Game.World.Characters;
using Code.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Hud.Windows.Abilities
{
    public class AbilityButton : UIRadioButton
    {
        public struct Model
        {
            public Sprite Sprite;
            public int Ability_Index;
        }
        
        public int Ability_Index { get; private set; }
        
        [SerializeField] private Image _icon;

        
        
        public void SetAbilityModel(Model model)
        {
            Ability_Index = model.Ability_Index;
            _icon.sprite = model.Sprite;
        }
    }
}