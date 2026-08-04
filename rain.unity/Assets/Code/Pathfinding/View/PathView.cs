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

        [SerializeField] private Color _newPathColor;
        [SerializeField] private Color _oldPathColor;

        [SerializeField] private Color _selectedTileUnWalcableColor;
        [SerializeField] private Color _selectedTileWalcableColor;
        
        
        public void DrawPath(List<Vector3> path, int currentPointIndex)
        {
            _pathPointsPool.DisableAll();

            for (int i = 0; i < path.Count; i++)
            {
                Vector3 vector3 = path[i];
                PathfindingPointView point = _pathPointsPool.GetNext();
                point.transform.position = vector3;
                point.SetColor(i >= currentPointIndex ? _newPathColor : _oldPathColor);
            }
        }

        public void UpdateSelectTile(Vector3 position, bool isWalkable)
        {
            _selectedTileBody.position = position;
            _selectTileRender.color = isWalkable ? _selectedTileWalcableColor : _selectedTileUnWalcableColor;
        }
    }
}