using System.Collections.Generic;
using Essential;
using UnityEngine;

namespace Code.Pathfinding
{
    public class PathDrawer : Essential.Mono, IPathfindingDrawer
    {
        [SerializeField] private PathfindingTilemapView _tilemap;
        [SerializeField] private Transform _selectedTileBody;
        [SerializeField] private SpriteRenderer _selectTileRender;
        [Space]
        [SerializeField] private MonoPool<PathfindingPointView> _pathPointsPool;

        [SerializeField] private Color _newPathColor;
        [SerializeField] private Color _oldPathColor;

        [SerializeField] private Color _selectedTileUnWalcableColor;
        [SerializeField] private Color _selectedTileWalcableColor;

        
        public void DrawPath(List<Vector3Int> path, int currentPointIndex)
        {
            _pathPointsPool.DisableAll();

            for (int i = 0; i < path.Count; i++)
            {
                Vector3Int vector3 = path[i];
                PathfindingPointView point = _pathPointsPool.GetNext();
                point.transform.position = _tilemap.CellAsCenterWorld(vector3);
                point.SetColor(i >= currentPointIndex ? _newPathColor : _oldPathColor);
            }
        }

        public void UpdateSelectTile(Vector3Int position, bool isWalkable)
        {
            _selectedTileBody.position = _tilemap.CellAsCenterWorld(position);
            _selectTileRender.color = isWalkable ? _selectedTileWalcableColor : _selectedTileUnWalcableColor;
        }
    }
}