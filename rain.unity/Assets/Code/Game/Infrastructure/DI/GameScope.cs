using Code.Game.Characters;
using Code.Game.Characters.Hero;
using Code.Game.Factories;
using Code.Game.StateMachine;
using Code.Game.World;
using Code.Game.World.Abilities;
using Code.Game.World.Characters;
using Code.Game.World.Turns;
using Code.Pathfinding;
using Core.Input;
using TriInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Game.DI
{
    public class GameScope : LifetimeScope
    {
        [Title("Path")]
        [SerializeField] private WeightTilesConfiguration _weightTilesConfiguration;
        [SerializeField] private TilemapView tilemapView;
        [SerializeField] private TileDrawer tileDrawer;

        [Title("Characters")] 
        [SerializeField] private ArmoryLibrary _armoryLibrary;
        [SerializeField] private AbilityLibrary _abilityLibrary;
        
        
        protected override void Configure(IContainerBuilder builder)
        {
            //game
            builder.Register<PlayerInputService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GamePhaseService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WorldActorRegistry>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<IEntityRegistry, EntityRegistry>(Lifetime.Singleton);
            
            //characters
            builder.RegisterInstance(_armoryLibrary).AsSelf();
            builder.RegisterInstance(_abilityLibrary).AsSelf();
            builder.Register<ICharacterFactory, CharacterFactory>(Lifetime.Singleton);
            builder.Register<PlayerSelection>(Lifetime.Singleton);
            builder.Register<PlayerPartyProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AbilityTargetingController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();

            //path
            builder.RegisterInstance(_weightTilesConfiguration).AsSelf();
            builder.RegisterInstance(tilemapView).AsImplementedInterfaces();
            builder.RegisterInstance(tileDrawer).AsImplementedInterfaces();
            
            builder.Register<PathfindingService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PlayerPathController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PlayerPathDrawer>(Lifetime.Singleton).AsImplementedInterfaces();

            //world
            builder.Register<CameraFollow>(Lifetime.Singleton).AsImplementedInterfaces();
            
            //entry point
            builder.RegisterEntryPoint<GameWorldBootstrap>();
        }
    }
}