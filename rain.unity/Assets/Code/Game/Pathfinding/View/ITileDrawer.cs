using System.Collections.Generic;
using UnityEngine;

namespace Code.Pathfinding
{
    public interface ITileDrawer
    {
        void DrawZone(IReadOnlyList<Vector3Int> zone);
        void DrawPath(IReadOnlyList<Vector3Int> path, int currentPointIndex);
        void UpdateSelectTile(Vector3Int position, bool isWalkable);
        void Clear();
    }
}