using Code.Game.World.Turns;
using UnityEngine;
using VContainer;
using VContainer.Unity;

#if UNITY_EDITOR
using TriInspector;
using UnityEditor;
#endif


namespace Code.Game.World
{
    public class ObstacleProvider : Essential.Mono, IStartable
    {
        [SerializeField] private Obstacle[] _obstacles;
        
        private IWorldActorRegistry _worldActorRegistry;


        [Inject]
        public void Construct(IWorldActorRegistry worldActorRegistry)
        {
            _worldActorRegistry = worldActorRegistry;
        }
        
        public void Start()
        {
            foreach (Obstacle obstacle in _obstacles)
            {
                _worldActorRegistry.Register(obstacle);
            }
        }

#if UNITY_EDITOR
        [Button]
        private void _findObstaclesInChild()
        {
            _obstacles = GetComponentsInChildren<Obstacle>();
            EditorUtility.SetDirty(this);
        }
#endif
    }
}