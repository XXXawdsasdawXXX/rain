using System.Collections.Generic;
using UnityEngine;

namespace Code.Pathfinding
{
    public interface IPathfindingDrawer
    {
        void DrawPath(List<Vector3Int> path, int currentPointIndex);
        void UpdateSelectTile(Vector3Int position, bool isWalkable);
    }
}