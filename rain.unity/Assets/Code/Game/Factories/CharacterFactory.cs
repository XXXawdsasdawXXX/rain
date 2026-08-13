using Code.Game.Characters;
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
        private readonly IPlayerPartyProvider _playerPartyProvider;
        private readonly GameSaveRepository _gameSaveRepository;
        private readonly IWorldActorRegistry _worldActorRegistry;
        private readonly IPathfindingService _pathfindingService;


        public CharacterFactory(
            IObjectResolver resolver, 
            AssetLibrary assetLibrary, 
            IPlayerPartyProvider playerPartyProvider, 
            GameSaveRepository gameSaveRepository,
            IWorldActorRegistry worldActorRegistry, 
            IPathfindingService pathfindingService)
        {
            _resolver = resolver;
            _assetLibrary = assetLibrary;
            _playerPartyProvider = playerPartyProvider;
            _gameSaveRepository = gameSaveRepository;
            _worldActorRegistry = worldActorRegistry;
            _pathfindingService = pathfindingService;
        }

        public CharacterWorldActor SpawnHero(CharacterModel model)
        {
            CharacterView heroPrefab = _assetLibrary.SceneComponents
                .Get(AssetsKey.CHARACTER)
                .GetComponent<CharacterView>();
            
            CharacterView heroInstance = _resolver.Instantiate(
                heroPrefab, 
                model.Position.AsVector3(), 
                Quaternion.identity);
            
            heroInstance.SetModel(model);

            IPathfindingFollower follower = new CharacterPathfindingFollower(_pathfindingService, heroInstance.AgentSettings); 
            CharacterWorldActor actor = new(heroInstance, follower);
            
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
            CharacterView npcPrefab = _assetLibrary.SceneComponents
                .Get(AssetsKey.CHARACTER)
                .GetComponent<CharacterView>();
            
            CharacterView npcInstance = _resolver.Instantiate(
                npcPrefab, 
                model.Position.AsVector3(), 
                Quaternion.identity);
            
            npcInstance.SetModel(model);
            
            GameModel gameSave = _gameSaveRepository.GetCurrentGame();

            if (!gameSave.Npc.Exists(h => h.ID.Equals(model.ID)))
            { 
                gameSave.Npc.Add(model);
            }
            
            IPathfindingFollower follower = new CharacterPathfindingFollower(_pathfindingService, npcInstance.AgentSettings); 
            CharacterWorldActor actor = new(npcInstance, follower);
            
            _worldActorRegistry.Register(actor);

            return actor;
        }
    }
}