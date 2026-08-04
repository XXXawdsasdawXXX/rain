using System.Collections.Generic;
using Essential;
using UnityEngine;

namespace Code.Pathfinding
{
    public class CharacterPathfindingFollower : IPathfindingFollower
    {
        private readonly IPathfindingService _pathfinding;
        public int PointIndex => _pathIndex;
        public List<Vector3> Path { get; private set; } = new();
        public bool HasActivePath { get; private set; }

        private int _pathIndex;
      
        private readonly PathfindingAgentSettings _agentSettings;
        private Vector3 _end;


        public CharacterPathfindingFollower(IPathfindingService pathfinding, PathfindingAgentSettings agentSettings)
        {
            _pathfinding = pathfinding;
            _agentSettings = agentSettings;
        }
        
        public void SetPath(Vector3 start, Vector3 end)
        {
            Path = new List<Vector3>(_pathfinding.FindPath(start, end, _agentSettings));
            _end = end;
            
            _pathIndex = 1;
            HasActivePath = Path.Count > 1;
        }
        

        public bool TryMoveToNextWorldPosition(out Vector3 nextPosition)
        {
            if (!HasActivePath || _pathIndex >= Path.Count)
            {
                HasActivePath = false;
                
                nextPosition = Path.Count > 0 ? Path[^1] : Vector3.zero;
          
                return false;
            }
    
            nextPosition = Path[_pathIndex];

            if (PathfindingCollisionChecker.IsBlocked(nextPosition, _agentSettings))
            {
                SetPath(Path[_pathIndex - 1], _end);

                if (!HasActivePath)
                {
                    return false;
                }
                
                nextPosition = Path[_pathIndex];
            }
            
            _pathIndex++;
            
            HasActivePath = _pathIndex < Path.Count;
    
            Log.Info(this, $"[MoveToNextWorldPosition] step to {nextPosition}");
    
            return true;
        }
    }
}