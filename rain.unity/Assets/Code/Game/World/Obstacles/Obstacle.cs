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
        [SerializeField, GUIColor(0.5f, 0.7f, 0.9f, 0.8f)] private ObstacleRender render;
        [SerializeField] private MatrixTriggerZone _triggerZone;

        private readonly List<IAlive> _aliveBuffer = new();

        public void Enable()
        {
            render.PlayIdle();
            _logic?.Bind(this);
        }

        public void OnWorldTurnBegin()
        {
            _logic?.Step();
        }

        public List<IAlive> GetAlive()
        {
            _aliveBuffer.Clear();

            if (_triggerZone != null)
            {
                _triggerZone.OverlapAlive(_aliveBuffer);
            }

            return _aliveBuffer;
        }

        public void Disable()
        {
            _logic.Dispose();
            render.Dispose();
            _aliveBuffer.Clear();
        }
    }
}
