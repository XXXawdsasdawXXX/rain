using System.Collections.Generic;
using UnityEngine;

namespace Code.Pathfinding
{
    public interface IPathfindingFollower
    {
        List<Vector3> Path { get; }
        bool HasActivePath { get; }
        bool TryMoveToNextWorldPosition(out Vector3 nextPosition);
        void SetPath(Vector3 start, Vector3 end);
    }
}