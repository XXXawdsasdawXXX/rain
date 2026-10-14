using Code.Game.World.Turns;
using Essential;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Game.World
{
    public class ObstaclePool : Essential.Mono, IStartable
    {
        [SerializeField] private MonoPool<Obstacle> _pool;
      
        private IWorldActorRegistry _actorRegistry;
        
        
        [Inject]
        private void Construct(IWorldActorRegistry actorRegistry)
        {
            _actorRegistry = actorRegistry;
        }

        public void Start()
        {
            var currentObstacle = _pool.GetAllEnabled();

            foreach (var obstacle in currentObstacle)
            {
                _actorRegistry.Register(obstacle);
            }
        }
    }
}