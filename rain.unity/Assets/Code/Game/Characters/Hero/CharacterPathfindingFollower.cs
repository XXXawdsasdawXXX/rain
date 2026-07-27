using System.Collections.Generic;
using Essential;
using UnityEngine;

namespace Code.Pathfinding
{
    public class CharacterPathfindingFollower : IPathfindingFollower
    {
        private readonly IPathfindingService _pathfinding;
        public bool HasActivePath { get; private set; }
        
        private List<Vector3> _path = new();
        private int _pathIndex;
      
        private Vector3 _start;
        private Vector3 _end;
        private PathfindingAgentSettings _agentSettings;


        public CharacterPathfindingFollower(IPathfindingService pathfinding, PathfindingAgentSettings agentSettings)
        {
            _pathfinding = pathfinding;
            _agentSettings = agentSettings;
        }
        
        public void SetPath(Vector3 start, Vector3 end)
        {
            _path = new List<Vector3>(_pathfinding.FindPath(start, end, _agentSettings));
            
            _start = start;
            _end = end;
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

            if (PathfindingCollisionChecker.IsBlocked(nextPosition, _agentSettings))
            {
                SetPath(_path[_pathIndex - 1], _end);

                if (!HasActivePath)
                {
                    return false;
                }
                
                nextPosition = _path[_pathIndex];
            }
            
            _pathIndex++;
            
            HasActivePath = _pathIndex < _path.Count;
    
            Log.Info(this, $"[MoveToNextWorldPosition] step to {nextPosition}");
    
            return true;
        }
    }
}