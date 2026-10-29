using System.Collections.Generic;
using UnityEngine;

namespace Code.Pathfinding
{
    public interface IPathfindingDrawer
    {
        void DrawPath(List<Vector3> path, int currentPointIndex);
        void UpdateSelectTile(Vector3 position, bool isWalkable);
    }
}