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
           
        }

        public void DrawPath(List<Vector3> path, int currentPointIndex)
        {
            _pathPointsPool.DisableAll();

            for (int i = 0; i < path.Count; i++)
            {
                Vector3 vector3 = path[i];
                PathfindingPointView point = _pathPointsPool.GetNext();
                point.transform.position = vector3;
                point.SetColor(i >= currentPointIndex ? Color.blue : Color.green);
            }
        }

        public void UpdateSelectTile(Vector3 position, bool isWalkable)
        {
            _selectedTileBody.position = position;
            _selectTileRender.color = isWalkable ? Color.white : Color.red;
        }
    }
}