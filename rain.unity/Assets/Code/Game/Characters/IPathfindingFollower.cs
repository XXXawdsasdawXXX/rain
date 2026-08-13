using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Pathfinding
{
    public interface IPathfindingFollower
    {
        event Action Updated;
        int PointIndex { get; }
        List<Vector3> Path { get; }
        bool HasActivePath { get; }
        bool TryMoveToNextWorldPosition(out Vector3 nextPosition);
        void SetPath(Vector3 start, Vector3 end);
        void Clear();
    }
}