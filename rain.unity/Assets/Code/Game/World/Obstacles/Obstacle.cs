using System;
using System.Collections.Generic;
using Code.Game.World.Turns;
using Essential;
using TriInspector;
using UnityEngine;

namespace Code.Game.World
{
    public class Obstacle : Essential.Mono, IWorldTurnBeginListener, IPoolEntity
    {
        [SerializeReference, GUIColor(0.8f, 0.7f, 0.9f, 0.8f)] private AObstacleLogic _logic;
        [SerializeField, GUIColor(0.5f, 0.7f, 0.9f, 0.8f)] private ObstacleRender _render;
        [SerializeField] private MatrixTriggerZone _triggerZone;

        private readonly List<IHealth> _livingBuffer = new();

        [ShowInInspector, ReadOnly] private EObstacleState _currentState;
        
        
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
                var entities = new List<IWorldEntityView>();
                
                _triggerZone.OverlapAlive(entities);
                
                foreach (IWorldEntityView entityView in entities)
                {
                    if (entityView.TryGetHealth(out IHealth health))
                    {
                        _livingBuffer.Add(health);
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
