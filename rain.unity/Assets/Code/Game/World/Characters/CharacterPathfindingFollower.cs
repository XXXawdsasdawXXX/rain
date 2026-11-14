using System;
using System.Collections.Generic;
using Code.Game.World.Turns;
using Essential;
using UnityEngine;

namespace Code.Pathfinding
{
    public class CharacterPathfindingFollower : IPathfindingFollower
    {
        public event Action Updated;
        public int PointIndex => _pathIndex;
        public List<Vector3Int> Path { get; private set; } = new();
        public bool HasActivePath { get; private set; }

        private readonly IPathfindingService _pathfinding;
        private readonly IEntityRegistry _entityRegistry;

        private Vector3Int _end;
        private int _pathIndex;


        public CharacterPathfindingFollower(
            IPathfindingService pathfinding, 
            IEntityRegistry entityRegistry)
        {
            _pathfinding = pathfinding;
            _entityRegistry = entityRegistry;
        }

        public void SetPath(Vector3Int start, Vector3Int end)
        {
            Path = new List<Vector3Int>(_pathfinding.FindPath(start, end));
            _end = end;
            
            _pathIndex = 1;
            HasActivePath = Path.Count > 1;
            
            Updated?.Invoke();
        }

        public Vector3 GetWorldPosition(Vector3Int cell)
        {
            return _pathfinding.Tilemap.CellAsCenterWorld(cell);
        }

        public void Clear()
        {
            Path.Clear();
            _pathIndex = 0;
            HasActivePath = false;
        }

        public bool TryMoveToNextCell(out Vector3Int nextPosition)
        {
            if (!HasActivePath || _pathIndex >= Path.Count)
            {
                HasActivePath = false;
                
                nextPosition = Path.Count > 0 ? Path[^1] : Vector3Int.zero;
          
                return false;
            }
    
            nextPosition = Path[_pathIndex];

            if (!_pathfinding.IsWalkable(nextPosition) || !_entityRegistry.IsEmptyCell(nextPosition))
            {
                SetPath(Path[_pathIndex - 1], _end);

                if (!HasActivePath)
                {
                    return false;
                }
                
                nextPosition = Path[_pathIndex];
                
                if (!_pathfinding.IsWalkable(nextPosition) || !_entityRegistry.IsEmptyCell(nextPosition))
                {
                    Clear();
                    return false;
                }
            }
            
            _pathIndex++;
            
            HasActivePath = _pathIndex < Path.Count;
    
            Updated?.Invoke();
    
            Log.Info(this, $"[MoveToNextWorldPosition] step to {nextPosition}");
            
            return true;
        }
    }
}