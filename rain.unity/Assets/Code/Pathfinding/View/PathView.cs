using System.Collections.Generic;
using Essential;
using UnityEngine;

namespace Code.Pathfinding
{
    public class PathView : Essential.Mono, IPathfindingDrawer
    {
        [SerializeField] private Transform _selectedTileBody;
        [SerializeField] private SpriteRenderer _selectTileRender;
        [Space]
        [SerializeField] private MonoPool<PathfindingPointView> _pathPointsPool;

        
        public void DrawPath(List<Vector3> path, bool isWalkable)
        {
            _pathPointsPool.DisableAll();

            foreach (Vector3 vector3 in path)
            {
                PathfindingPointView point = _pathPointsPool.GetNext();
                point.transform.position = vector3;
                point.SetColor(isWalkable ? Color.white : Color.red); 
            }
        }

        public void UpdateSelectTile(Vector3 position, bool isWalkable)
        {
            _selectedTileBody.position = position;
            _selectTileRender.color = isWalkable ? Color.white : Color.red;
        }
    }
}