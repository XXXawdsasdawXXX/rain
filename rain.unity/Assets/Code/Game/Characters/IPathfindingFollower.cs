using System.Collections.Generic;
using UnityEngine;

namespace Code.Pathfinding
{
    public interface IPathfindingFollower
    {
        bool HasActivePath { get; }
        bool TryMoveToNextWorldPosition(out Vector3 nextPosition);
        void SetPath(IReadOnlyList<Vector3> path);
    }
}