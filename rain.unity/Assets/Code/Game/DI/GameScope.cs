using Code.Game.Characters;
using Code.Game.Characters.Hero;
using Code.Game.Factories;
using Code.Game.StateMachine;
using Code.Pathfinding;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Game.DI
{
    public class GameScope : LifetimeScope
    {
        [SerializeField] private WeightTilesConfiguration _weightTilesConfiguration;
        [SerializeField] private PathfindingTilemapView _pathfindingTilemapView;

        protected override void Configure(IContainerBuilder builder)
        {
            //pathfinding
            builder.RegisterInstance(_weightTilesConfiguration).AsSelf();
            builder.RegisterInstance(_pathfindingTilemapView).AsSelf();
            
            builder.Register<PathfindingService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PlayerPartyProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PlayerPartyMovement>(Lifetime.Singleton).AsImplementedInterfaces();

            //hero
            builder.Register<IHeroFactory, HeroFactory>(Lifetime.Singleton);
            builder.Register<IHeroProvider, HeroProvider>(Lifetime.Singleton);
            
            //entry point
            builder.RegisterEntryPoint<GameWorldBootstrap>();
        }
        
    }
}