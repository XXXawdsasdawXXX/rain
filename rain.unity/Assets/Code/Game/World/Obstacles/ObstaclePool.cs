using System.Collections.Generic;
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
      
        private IRegister<IWorldTurnBeginListener> _actorRegistry;
        
        
        [Inject]
        private void Construct(IRegister<IWorldTurnBeginListener> actorRegistry)
        {
            _actorRegistry = actorRegistry;
        }

        public void Start()
        {
            List<Obstacle> currentObstacle = _pool.GetAllEnabled();

            foreach (Obstacle obstacle in currentObstacle)
            {
                _actorRegistry.Register(obstacle);
            }
        }
    }
}