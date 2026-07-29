using System.Collections.Generic;
using Code.Game.Factories;
using Code.Game.Save;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Code.Game.StateMachine
{
    [UsedImplicitly]
    public sealed class GameWorldBootstrap : IStartable
    {
        private readonly GameSaveRepository _gameSave;
        private readonly ICharacterFactory _characterFactory;

        
        public GameWorldBootstrap(GameSaveRepository gameSave, ICharacterFactory characterFactory)
        {
            _gameSave = gameSave;
            _characterFactory = characterFactory;
        }
        
        public void Start()
        {
            _createHero();
            _createNpc();
        }

        private void _createHero()
        {
            List<CharacterModel> heroes = _gameSave.GetCurrentGame().Heroes;

            foreach (CharacterModel characterModel in heroes)
            {
                _characterFactory.SpawnHero(characterModel);
            }
        }

        private void _createNpc()
        {
            List<CharacterModel> npc = _gameSave.GetCurrentGame().Npc;

            foreach (CharacterModel characterModel in npc)
            {
                _characterFactory.SpawnNpc(characterModel);
            }
        }
    }
}