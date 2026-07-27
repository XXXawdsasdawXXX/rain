using System.Collections.Generic;
using UnityEngine;

namespace Code.Pathfinding
{
    public interface IPathfindingDrawer
    {
        void DrawPath(Vector3Int target, List<Vector3Int> path, bool isWalkable);
    }
}