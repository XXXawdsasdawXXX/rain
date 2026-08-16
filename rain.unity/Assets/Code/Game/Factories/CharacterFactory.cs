using Code.Game.Characters;
using Code.Game.Characters.Armor;
using Code.Game.Characters.Hero;
using Code.Game.Save;
using Code.Game.World.Turns;
using Code.Pathfinding;
using Core.AssetManagement.Libraries.Assets;
using Essential.Extensions;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Game.Factories
{
    [UsedImplicitly]
    public sealed class CharacterFactory : ICharacterFactory
    {
        private readonly IObjectResolver _resolver;
        private readonly AssetLibrary _assetLibrary;
        private readonly ArmoryLibrary _armoryLibrary;
        private readonly IPlayerPartyProvider _playerPartyProvider;
        private readonly GameSaveRepository _gameSaveRepository;
        private readonly IWorldActorRegistry _worldActorRegistry;
        private readonly IPathfindingService _pathfindingService;


        public CharacterFactory(
            IObjectResolver resolver, 
            AssetLibrary assetLibrary, 
            ArmoryLibrary armoryLibrary,
            IPlayerPartyProvider playerPartyProvider, 
            GameSaveRepository gameSaveRepository,
            IWorldActorRegistry worldActorRegistry, 
            IPathfindingService pathfindingService)
        {
            _resolver = resolver;
            _assetLibrary = assetLibrary;
            _armoryLibrary = armoryLibrary;
            _playerPartyProvider = playerPartyProvider;
            _gameSaveRepository = gameSaveRepository;
            _worldActorRegistry = worldActorRegistry;
            _pathfindingService = pathfindingService;
        }

        public CharacterWorldActor SpawnHero(CharacterModel model)
        {
            WorldCharacterRender heroPrefab = _assetLibrary.SceneComponents
                .Get(AssetsKey.CHARACTER)
                .GetComponent<WorldCharacterRender>();
            
            WorldCharacterRender heroInstance = _resolver.Instantiate(
                heroPrefab, 
                model.Position.AsVector3(), 
                Quaternion.identity);
            
            IPathfindingFollower follower = new CharacterPathfindingFollower(_pathfindingService, heroInstance.AgentSettings); 
            CharacterWorldActor actor = new(model, heroInstance, follower, _armoryLibrary);
            _playerPartyProvider.AddHero(actor);

            GameModel gameSave = _gameSaveRepository.GetCurrentGame();

            if (!gameSave.Heroes.Exists(h => h.ID.Equals(model.ID)))
            { 
                gameSave.Heroes.Add(model);
            }
            
            _worldActorRegistry.Register(actor);
            
            return actor;
        }

        public CharacterWorldActor SpawnNpc(CharacterModel model)
        {
            WorldCharacterRender npcPrefab = _assetLibrary.SceneComponents
                .Get(AssetsKey.CHARACTER)
                .GetComponent<WorldCharacterRender>();
            
            WorldCharacterRender npcInstance = _resolver.Instantiate(
                npcPrefab, 
                model.Position.AsVector3(), 
                Quaternion.identity);

            GameModel gameSave = _gameSaveRepository.GetCurrentGame();

            if (!gameSave.Npc.Exists(h => h.ID.Equals(model.ID)))
            { 
                gameSave.Npc.Add(model);
            }
            
            IPathfindingFollower follower = new CharacterPathfindingFollower(_pathfindingService, npcInstance.AgentSettings); 
            CharacterWorldActor actor = new(model, npcInstance, follower, _armoryLibrary);
            
            _worldActorRegistry.Register(actor);

            return actor;
        }
    }
}