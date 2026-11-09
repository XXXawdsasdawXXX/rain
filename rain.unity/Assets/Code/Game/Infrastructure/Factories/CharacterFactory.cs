using System;
using System.Collections.Generic;
using Code.Game.Characters;
using Code.Game.Characters.Armor;
using Code.Game.Characters.Hero;
using Code.Game.Save;
using Code.Game.World;
using Code.Game.World.Turns;
using Code.Pathfinding;
using Core.AssetManagement.Libraries.Assets;
using JetBrains.Annotations;
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
        private readonly IRegister<IWorldTurnBeginListener> _worldActorRegistry;
        private readonly IEntityRegistry _entityRegistry;
        private readonly IPathfindingService _pathfindingService;


        public CharacterFactory(
            IObjectResolver resolver, 
            AssetLibrary assetLibrary, 
            ArmoryLibrary armoryLibrary,
            IPlayerPartyProvider playerPartyProvider, 
            GameSaveRepository gameSaveRepository,
            IRegister<IWorldTurnBeginListener> worldActorRegistry, 
            IEntityRegistry entityRegistry, 
            IPathfindingService pathfindingService)
        {
            _resolver = resolver;
            _assetLibrary = assetLibrary;
            _armoryLibrary = armoryLibrary;
            _playerPartyProvider = playerPartyProvider;
            _gameSaveRepository = gameSaveRepository;
            _worldActorRegistry = worldActorRegistry;
            _entityRegistry = entityRegistry;
            _pathfindingService = pathfindingService;
        }

        public CharacterActor Spawn(EEntityType type, CharacterModel model)
        {
            switch (type)
            {
                case EEntityType.None:
                case EEntityType.Enemy:
                case EEntityType.Obstacle:
                default:
                    throw new Exception($"factory can not spawn actor with type {type}");
                
                case EEntityType.Hero:
                    return SpawnHero(model);
                
                case EEntityType.Npc:
                    return SpawnNpc(model);
            }
        }

        public CharacterActor SpawnHero(CharacterModel model)
        {
            if (model == null)
            {
                throw new Exception("model for spawn hero is null");
            }
            
            CharacterWorldView heroInstance = _create(model, out CharacterActor actor);
            
            _playerPartyProvider.AddHero(actor);
            
            _register(EEntityType.Hero, heroInstance, actor);
            
            return actor;
        }

        public CharacterActor SpawnNpc(CharacterModel model)
        {
            if (model == null)
            {
                throw new Exception("model for spawn npc is null");
            }
            
            CharacterWorldView npcInstance = _create(model, out CharacterActor actor);

            _register(EEntityType.Npc, npcInstance, actor);
            
            return actor;
        }

        private CharacterWorldView _create(CharacterModel model, out CharacterActor actor)
        {
            CharacterWorldView npcPrefab = _assetLibrary.SceneComponents
                .Get(AssetsKey.CHARACTER)
                .GetComponent<CharacterWorldView>();

            CharacterWorldView npcInstance = _resolver.Instantiate(npcPrefab);
            npcInstance.SetCellPosition(model.Position);

            IPathfindingFollower follower = new CharacterPathfindingFollower(_pathfindingService, _entityRegistry);
            actor = new CharacterActor(model, npcInstance, follower, _armoryLibrary);
            return npcInstance;
        }

        private void _register(EEntityType entityType, CharacterWorldView view, CharacterActor actor)
        {
            view.SetActor(actor);
            view.SetEntityType(entityType);
            
            _entityRegistry.Register(view);
            _worldActorRegistry.Register(actor);
            
            if (_gameSaveRepository.TryGetCurrentGame(out GameModel gameModel))
            {
                if (gameModel.Characters.TryGetValue(entityType, out List<CharacterModel> collection))
                {
                    if (!collection.Exists(h => h.ID.Equals(actor.Model.ID)))
                    { 
                        collection.Add(actor.Model);
                    }
                }
            }
        }
    }
}