using System.Linq;
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
        [field: SerializeField] public MonoPool<HeroTabView> HeroesTab { get; private set; }
        
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

            foreach (CharacterView partyCharacter in _playerParty.Characters)
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
        }

        protected override void unSubscribe()
        {
            _playerParty.CharacterAdded -= _addHeroTab;
            _playerParty.CharacterSelected -= _selectHero;
        }

        private void _addHeroTab(CharacterView characterView)
        {
            HeroTabView tab = view.HeroesTab.GetNext();
            tab.SetCharacterModel(characterView.Model);
        }

        private void _selectHero(CharacterView characterView)
        {
            foreach (HeroTabView tab in view.HeroesTab.GetAllEnabled())
            {
                tab.Deselect();
            }

            if (characterView == null)
            {
                return;
            }
            
            view.HeroesTab.GetAllEnabled()
                ?.FirstOrDefault(t => t.Model.ID == characterView.Model.ID)
                ?.Select();
        }
    }
}