using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Pathfinding
{
    public interface IPathfindingFollower
    {
        event Action Updated;
        int PointIndex { get; }
        List<Vector3Int> Path { get; }
        bool HasActivePath { get; }
        bool TryMoveToNextCell(out Vector3Int nextCell);
        Vector3 GetWorldPosition(Vector3Int cell);
        void SetPath(Vector3Int start, Vector3Int end);
        void Clear();
    }
}