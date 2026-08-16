using System.Collections.Generic;
using Code.Game.World.Turns;
using Essential.Extensions;
using UnityEngine;

namespace Code.Game.World
{
    public class Obstacle : Essential.Mono, IWorldTurnBeginListener
    {
        [field: SerializeField] public EObstacleLogic ObstacleLogic { get; private set; }
        
        [SerializeField] private BoxCollider2D _collider;
        public  List<IAlive> AliveInside { get; private set; }= new List<IAlive>();
        
        private IObstacleLogic _logic;    
        
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out IAlive alive) && !AliveInside.Contains(alive))
            {
                AliveInside.Add(alive);
            }
            
            Debug.Log($"[OnTriggerEnter2D] {other.name}");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out IAlive alive) && AliveInside.Contains(alive))
            {
                AliveInside.Remove(alive);
            }
            Debug.Log($"[OnTriggerExit2D] {other.name}");
        }

        public void OnWorldTurnBegin()
        {
            _logic?.Step();
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (_collider == null)
            {
                return;
            }
            
            Gizmos.color = ColorExtension.OBSTACLE_GIZMO_COLOR;
            
            Gizmos.DrawCube(transform.position + _collider.offset.AsVector3(), _collider.size);
        }
#endif
    }

}