using Code.Game.Characters.Hero;
using Code.Game.World.Characters;
using Code.UI.Components;
using Code.UI.Components.Window;
using UnityEngine;

namespace Code.UI.Hud.Windows
{
    public class SelectHeroView : AUIWindowView
    {
        [field: SerializeField] public UIText TextHeroName { get; private set; }
        [field: SerializeField] public UIArmorRender ArmorRender { get; private set; } 
        [field: SerializeField] public UICharacterStats Stats { get; private set; }       
        
    }

    public class SelectHeroViewController : AUIWindowController<SelectHeroView>
    {
        private readonly IPlayerPartyProvider _partyProvider;
        
        
        public SelectHeroViewController(
            SelectHeroView view, 
            ArmoryLibrary armoryLibrary,
            IPlayerPartyProvider partyProvider) : base(view)
        {
            this.view.ArmorRender.SetArmorLibrary(armoryLibrary);
            _partyProvider = partyProvider;
        }

        public override void Start()
        {
            base.Start();
            
            _onSelectHero(_partyProvider.CurrentCharacter);
        }

        protected override void subscribe()
        {
            _partyProvider.HeroSelected += _onSelectHero;
        }

        protected override void unSubscribe()
        {
            _partyProvider.HeroSelected -= _onSelectHero;
            
            view.ArmorRender.DisposeHero();
            view.Stats.DisposeHero();
        }

        private void _onSelectHero(CharacterActor hero)
        {
            if (hero == null)
            {
                return;
            }
            
            view.TextHeroName.SetText(hero.Model.Name);
            view.ArmorRender.SetHero(hero);
            view.Stats.SetHero(hero);
        }
    }
}