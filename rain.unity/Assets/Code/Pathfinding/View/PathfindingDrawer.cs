using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Pathfinding
{
    [UsedImplicitly]
    public sealed class PathfindingDrawer : IPathfindingDrawer
    {
        private readonly PathView _pathView;

        
        public PathfindingDrawer(PathView pathView)
        {
            _pathView = pathView;
        }
        
        public void DrawPath(Vector3Int target, List<Vector3Int> path, bool isWalkable)
        {
            _pathView.SetSelectTile(target);
            
            if (path != null && path.Count > 0)
            {
                _pathView.SetPath(path, isWalkable);
            }
        }
    }
}