using System.Collections.Generic;
using UnityEngine;

namespace Code.Pathfinding
{
    public static class PathfindingTools
    {
        public static readonly Dictionary<Vector3Int, int> NeighborOffsets = new()
        {
            { new Vector3Int(-1, 0, 0), 10 }, // влево
            { new Vector3Int(1, 0, 0), 10 }, // вправо
            { new Vector3Int(0, -1, 0), 10 }, // вниз
            { new Vector3Int(0, 1, 0), 10 }, // вверх
            { new Vector3Int(-1, -1, 0), 14 }, // ↖
            { new Vector3Int(-1, 1, 0), 14 }, // ↗
            { new Vector3Int(1, -1, 0), 14 }, // ↙
            { new Vector3Int(1, 1, 0), 14 } // ↘
        };
    }
}