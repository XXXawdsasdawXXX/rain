using Code.Game.World.Turns;
using Essential;
using UnityEngine;
using VContainer;

namespace Code.Game.World
{
    public class ObstaclePool : Essential.Mono
    {
        [SerializeField] private MonoPool<Obstacle> _pool;
      
        private IWorldActorRegistry _actorRegistry;
        
        
        [Inject]
        private void Construct(IWorldActorRegistry actorRegistry)
        {
            _actorRegistry = actorRegistry;
        }
    }
}