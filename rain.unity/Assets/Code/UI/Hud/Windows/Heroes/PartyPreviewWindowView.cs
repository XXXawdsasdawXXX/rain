using Code.Game.Characters;
using Code.Game.Characters.Armor;
using Code.Game.Characters.Hero;
using Code.UI.Components;
using Code.UI.Components.Window;
using Essential;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.UI.Hud.Windows.Heroes
{
    public class PartyPreviewWindowView : AUIWindowView
    {
        [field: SerializeField] public UIRadioGroup RadioGroupHeroes { get; private set; }
    }

    [UsedImplicitly]
    public class PartyPreviewWindowController : AUIWindowController<PartyPreviewWindowView>
    {
        private readonly IPlayerPartyProvider _playerParty;
        private readonly ArmoryLibrary _armoryLibrary;

        public PartyPreviewWindowController(
            PartyPreviewWindowView view, 
            IPlayerPartyProvider playerParty,
            ArmoryLibrary armoryLibrary) : base(view)
        {
            _playerParty = playerParty;
            _armoryLibrary = armoryLibrary;
        }

        public override void Start()
        {
            base.Start();

            foreach (CharacterWorldActor partyCharacter in _playerParty.Heroes)
            {
                _addHeroTab(partyCharacter);
            }
            
            _selectHero(_playerParty.CurrentCharacter);
        }

        protected override void subscribe()
        {
            _playerParty.CharacterAdded += _addHeroTab;
            _playerParty.HeroSelected += _selectHero;

            view.RadioGroupHeroes.Checked += _onChangeSelectedHeroes;
        }

        protected override void unSubscribe()
        {
            _playerParty.CharacterAdded -= _addHeroTab;
            _playerParty.HeroSelected -= _selectHero;
            
            view.RadioGroupHeroes.Checked -= _onChangeSelectedHeroes;
        }

        private void _addHeroTab(CharacterWorldActor character)
        {
            HeroGroupTabView groupTab = view.RadioGroupHeroes.AddElement<HeroGroupTabView>();
           
            if (groupTab == null)
            {
                Log.Error(this, $"[_addHeroTab] can`t find component 'hero tab view'");
                return;
            }
            
            groupTab.SetLibrary(_armoryLibrary);
            groupTab.SetHero(character);
        }

        //where player party change selected heroes from world
        private void _selectHero(CharacterWorldActor character)
        {
            if (character == null)
            {
                return;
            }

            view.RadioGroupHeroes.SetCheckedWithoutNotify(_playerParty.Heroes.IndexOf(character));
        }

        //where change selected hero from ui
        private void _onChangeSelectedHeroes(int heroIndex)
        {
            if (heroIndex >= _playerParty.Heroes.Count)
            {
                Log.Error(this, $"[_onChangeSelectedHeroes] hero index >= heroes count");
            }
            
            _playerParty.SetCurrentCharacter(_playerParty.Heroes[heroIndex]);
        }
    }
}