using Code.Game.Characters;
using Code.Game.Characters.Hero;
using Code.UI.Components.Window;
using Essential;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.UI.Hud.Windows
{
    public class HeroesListWindowView : AUIWindowView
    {
        [field: SerializeField] public UIRadioGroup RadioGroupHeroes { get; private set; }
    }

    [UsedImplicitly]
    public class HeroesListWindowController : AUIWindowController<HeroesListWindowView>
    {
        private readonly IPlayerPartyProvider _playerParty;

        
        public HeroesListWindowController(HeroesListWindowView view, IPlayerPartyProvider playerParty) : base(view)
        {
            _playerParty = playerParty;
        }

        public override void Start()
        {
            base.Start();

            foreach (CharacterView partyCharacter in _playerParty.Heroes)
            {
                _addHeroTab(partyCharacter);
            }

            if (_playerParty.SelectedCharacters.Count > 0)
            {
                _selectHero(_playerParty.SelectedCharacters[0]);
            }
        }

        protected override void subscribe()
        {
            _playerParty.CharacterAdded += _addHeroTab;
            _playerParty.CharacterSelected += _selectHero;

            view.RadioGroupHeroes.Checked += _onChangeSelectedHeroes;
        }

        protected override void unSubscribe()
        {
            _playerParty.CharacterAdded -= _addHeroTab;
            _playerParty.CharacterSelected -= _selectHero;
            
            view.RadioGroupHeroes.Checked -= _onChangeSelectedHeroes;
        }

        private void _addHeroTab(CharacterView characterView)
        {
            HeroTabView tab = view.RadioGroupHeroes.AddRadioButton<HeroTabView>();
           
            if (tab == null)
            {
                Log.Error(this, $"[_addHeroTab] can`t find component 'hero tab view'");
                return;
            }
            
            tab.SetCharacterModel(characterView.Model);
        }

        //where player party change selected heroes from world
        private void _selectHero(CharacterView characterView)
        {
            if (characterView == null)
            {
                return;
            }

            view.RadioGroupHeroes.SetCheckedWithoutNotify(_playerParty.Heroes.IndexOf(characterView));
        }

        //where change selected hero from ui
        private void _onChangeSelectedHeroes(int heroIndex)
        {
            if (heroIndex >= _playerParty.Heroes.Count)
            {
                Log.Error(this, $"[_onChangeSelectedHeroes] hero index >= heroes count");
            }
            
            _playerParty.SetSelectedCharacter(_playerParty.Heroes[heroIndex]);
        }
    }
}