using Code.Game.World.Turns;
using UnityEngine;
using VContainer;

#if UNITY_EDITOR
using TriInspector;
using UnityEditor;
#endif


namespace Code.Game.World
{
    public class ObstacleProvider : Essential.Mono
    {
        [SerializeField] private Obstacle[] _obstacles;
        
        private IRegister<IWorldTurnBeginListener> _worldActorRegistry;


        [Inject]
        public void Construct(IRegister<IWorldTurnBeginListener> worldActorRegistry)
        {
            _worldActorRegistry = worldActorRegistry;
        }
        
        public void OnEnable()
        {
            if (_worldActorRegistry == null)
            {
                return;
            }
            
            foreach (Obstacle obstacle in _obstacles)
            {
                _worldActorRegistry.Register(obstacle);
            }
        }
        
        public void OnDisable()
        {
            if (_worldActorRegistry == null)
            {
                return;
            }
            
            foreach (Obstacle obstacle in _obstacles)
            {
                _worldActorRegistry.UnRegister(obstacle);
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