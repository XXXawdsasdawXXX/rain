using System.Collections.Generic;
using Code.Game.Characters;
using Code.Game.Characters.Hero;
using Code.Game.World.Abilities;
using Code.Game.World.Characters;
using Code.UI.Components;
using Code.UI.Components.Window;
using Code.UI.Hud.Windows.Abilities;
using Essential;
using UnityEngine;

namespace Code.UI.Hud.Windows
{
    public class SelectHeroView : AUIWindowView
    {
        [field: SerializeField] public UIText TextHeroName { get; private set; }
        [field: SerializeField] public UIArmorRender ArmorRender { get; private set; } 
        [field: SerializeField] public UICharacterStats Stats { get; private set; }       
        [field: SerializeField] public AbilitiesRadioGroupView Abilities { get; private set; }       
        
    }

    public class SelectHeroViewController : AUIWindowController<SelectHeroView>
    {
        private readonly AbilityLibrary _abilityLibrary;
        private readonly IPlayerPartyProvider _partyProvider;
        private readonly IAbilityTargeting _abilityTargetingController;


        public SelectHeroViewController(
            SelectHeroView view, 
            ArmoryLibrary armoryLibrary,
            AbilityLibrary abilityLibrary,
            IPlayerPartyProvider partyProvider,
            IAbilityTargeting abilityTargetingController) : base(view)
        {
            this.view.ArmorRender.SetArmorLibrary(armoryLibrary);
            _abilityLibrary = abilityLibrary;
            _partyProvider = partyProvider;
            _abilityTargetingController = abilityTargetingController;
        }

        public override void Start()
        {
            base.Start();
            
            _onSelectHero(_partyProvider.CurrentCharacter);
        }

        protected override void subscribe()
        {
            _partyProvider.HeroSelected += _onSelectHero;
            view.Abilities.Choiced += _onAbilityChoiced;
        }

        private void _onAbilityChoiced(AbilityModel abilityModel)
        {
            _abilityTargetingController.BeginTargeting(abilityModel);
        }

        protected override void unSubscribe()
        {
            _partyProvider.HeroSelected -= _onSelectHero;
            view.Abilities.Choiced -= _onAbilityChoiced;
            
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

            List<AbilityModel> abilityModels = new();

            Log.Info(this, $"[_on select hero] abilities count = {hero.Model.Abilities.Count}");
            
            foreach (int abilityIndex in hero.Model.Abilities)
            {
                abilityModels.Add(_abilityLibrary.Library.Get(abilityIndex));
            }
            
            view.Abilities.SetHero(hero, abilityModels);
        }
    }
}