using System;
using System.Collections.Generic;
using Code.Game.Factories;
using Code.Game.Save;
using Code.Game.World;
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
            if (_gameSave.TryGetCurrentGame(out GameModel game))
            {
                _createCharacters(EEntityType.Hero, game);
                _createCharacters(EEntityType.Npc, game);
            }
        }

        private void _createCharacters(EEntityType type, GameModel game)
        {
            if (!game.Characters.ContainsKey(type))
            {
                throw new Exception($"game model has not character collection for {EEntityType.Npc}");
            }
            
            List<CharacterModel> characterModels = game.Characters[type];

            foreach (CharacterModel characterModel in characterModels)
            {
                _characterFactory.Spawn(type, characterModel);
            }
        }
    }
}