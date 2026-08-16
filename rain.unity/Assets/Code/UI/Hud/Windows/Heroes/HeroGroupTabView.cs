using Code.Game.Characters;
using Code.Game.Characters.Armor;
using Code.UI.Components;
using UnityEngine;

namespace Code.UI.Hud.Windows.Heroes
{
    public class HeroGroupTabView : UIRadioButton
    {
        [SerializeField] private UIText _textHeroName;
        [SerializeField] private UIArmorRender _armor;
        [SerializeField] private UICharacterStats _stats;

        private CharacterWorldActor _actor;

        
        public void SetLibrary(ArmoryLibrary armoryLibrary)
        {
            _armor.SetArmorLibrary(armoryLibrary);
        }
        
        public void SetHero(CharacterWorldActor actor)
        {
            _disposeHero();
            
            if (actor == null)
            {
                return;
            }

            _actor = actor;
            
            _textHeroName.SetText(_actor.Model.Name);
            _stats.SetHero(_actor);
            _armor.SetHero(_actor);
        }

        
        private void _disposeHero()
        {
            _stats.DisposeHero();
            _armor.DisposeHero();
        }
    }
}   