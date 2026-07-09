using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Pathfinding
{
    public sealed class PathfindingScope : LifetimeScope
    {
        [SerializeField] private WeightTilesConfiguration _weightTilesConfiguration;
        [SerializeField] private PathfindingTilemapView _pathfindingTilemapView;
        
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_weightTilesConfiguration).AsSelf();
            builder.RegisterInstance(_pathfindingTilemapView).AsSelf();

            builder.Register<PathfindingService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}