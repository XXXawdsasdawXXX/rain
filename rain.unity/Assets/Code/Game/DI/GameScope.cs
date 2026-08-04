using Code.Game.Characters;
using Code.Game.Characters.Armor;
using Code.Game.Characters.Hero;
using Code.Game.Factories;
using Code.Game.StateMachine;
using Code.Game.World;
using Code.Pathfinding;
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
        [SerializeField] private PathfindingTilemapView _pathfindingTilemapView;
        [SerializeField] private PathView _pathView;

        [Title("Characters")] 
        [SerializeField] private ArmoryLibrary _armoryLibrary;
        
        
        protected override void Configure(IContainerBuilder builder)
        {
            //path
            builder.RegisterInstance(_weightTilesConfiguration).AsSelf();
            builder.RegisterInstance(_pathfindingTilemapView).AsSelf();
            builder.RegisterInstance(_pathView).AsImplementedInterfaces();
            
            builder.Register<PathfindingService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PlayerPartyProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PlayerPartyMovement>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PartyPathDrawer>(Lifetime.Singleton).AsImplementedInterfaces();

            //world
            builder.Register<CameraFollow>(Lifetime.Singleton).AsImplementedInterfaces();
            
            //characters
            builder.Register<ICharacterFactory, CharacterFactory>(Lifetime.Singleton);
            builder.Register<PlayerSelection>(Lifetime.Singleton);
            builder.RegisterInstance(_armoryLibrary).AsSelf();
            
            //entry point
            builder.RegisterEntryPoint<GameWorldBootstrap>();
        }
        
    }
}