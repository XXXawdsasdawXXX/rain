using System.Collections.Generic;
using UnityEngine;

namespace Code.Pathfinding
{
    public interface IPathfindingService
    {
        ITilemapView Tilemap { get; }
        IReadOnlyList<Vector3Int> FindPath(Vector3Int startPosition, Vector3Int endPosition);
        IReadOnlyList<Vector3Int> FindPath(Vector3 startPosition, Vector3 endPosition);
        bool IsWalkable(Vector3Int worldPosition);
    }
}