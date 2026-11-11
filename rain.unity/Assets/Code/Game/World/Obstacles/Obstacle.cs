using System.Collections.Generic;
using Code.Game.World.Turns;
using Essential;
using TriInspector;
using UnityEngine;
using VContainer;

namespace Code.Game.World
{
    public class Obstacle : WorldMono, IWorldTurnBeginListener, IPoolEntity
    {
        public IWorldTurnBeginListener.ETurnPriority Priority =>
            IWorldTurnBeginListener.ETurnPriority.Obstacle;

        [SerializeReference, GUIColor(0.8f, 0.7f, 0.9f, 0.9f)]
        private AObstacleLogic _logic;
        
        [SerializeField, GUIColor(0.5f, 0.7f, 0.9f, 0.9f)] 
        private ObstacleRender _render;
        
        [SerializeField] private MatrixTriggerZone _triggerZone;
        
        [ShowInInspector, ReadOnly] private EObstacleState _currentState;
        
        private readonly List<IHealth> _livingBuffer = new();
        
        private IEntityRegistry _entityRegistry;

        
        [Inject]
        public void Construct(IEntityRegistry entityRegistry)
        {
            _entityRegistry = entityRegistry;
        }
        
        private void Awake()
        {
            _logic?.Bind(this);
        }

        public void Enable()
        {
            _currentState = EObstacleState.Idle;
            _render.PlayIdle();
        }
        
        public void OnWorldTurnBegin()
        {
            EObstacleState state = _logic?.Step() ?? EObstacleState.Idle;

            if (state != _currentState)
            {
                switch (state)
                {
                    case EObstacleState.Idle:
                    default:
                        _render.PlayIdle();
                        break;
                    case EObstacleState.Reaction:
                        _render.PlayReaction();
                        break;
                    case EObstacleState.Destroyed:
                        _render.PlayDestroy();
                        break;
                }

                _currentState = state;
            }
        }

        public List<IHealth> GetAlive()
        {
            _livingBuffer.Clear();

            if (_triggerZone != null)
            {
                IReadOnlyList<Vector3Int> cells = _triggerZone.GetCells();

                foreach (Vector3Int cellPosition in cells)
                {
                    if (_entityRegistry.TryGetEntities(cellPosition, out List<IEntityView> entities))
                    {
                        foreach (IEntityView entity in entities)
                        {
                            if (entity.TryGetHealth(out IHealth health))
                            {
                                _livingBuffer.Add(health);
                            }
                        }

                    }
                }
            }
            
            return _livingBuffer;
        }

        public void Disable()
        {
            _logic.Dispose();
            _render.Dispose();
            _livingBuffer.Clear();
        }
    }
}
