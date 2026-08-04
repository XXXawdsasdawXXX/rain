using System.Collections.Generic;
using UnityEngine;

namespace Code.Pathfinding
{
    public interface IPathfindingService
    {
        IReadOnlyList<Vector3> FindPath(Vector3 startPosition, Vector3 endPosition, PathfindingAgentSettings settings);
        bool IsWalkable(Vector3 worldPosition, PathfindingAgentSettings agentSettings);
    }
}