using System.Collections.Generic;
using Essential;
using UnityEngine;

namespace Code.Pathfinding
{
    public class CharacterPathfindingFollower : IPathfindingFollower
    {
        public bool HasActivePath { get; private set; }
        
        private List<Vector3> _path = new();
        private int _pathIndex;

        
        public void SetPath(IReadOnlyList<Vector3> path)
        {
            _path = new List<Vector3>(path);
           
            _pathIndex = 1;
            
            HasActivePath = _path.Count > 1;
        }
        
        public bool TryMoveToNextWorldPosition(out Vector3 nextPosition)
        {
            if (!HasActivePath || _pathIndex >= _path.Count)
            {
                HasActivePath = false;
                
                nextPosition = _path.Count > 0 ? _path[^1] : Vector3.zero;
          
                return false;
            }
    
            nextPosition = _path[_pathIndex];
            
            _pathIndex++;
            
            HasActivePath = _pathIndex < _path.Count;
    
            Log.Info(this, $"[MoveToNextWorldPosition] step to {nextPosition}");
    
            return true;
        }
    }
}