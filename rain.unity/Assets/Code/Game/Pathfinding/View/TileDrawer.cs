using System.Collections.Generic;
using Code.Game.World;
using Essential;
using UnityEngine;

namespace Code.Game.Pathfinding.View
{
    public class TileDrawer : WorldMono, ITileDrawer
    {
        [SerializeField] private TilemapView _tilemap;
        [SerializeField] private Transform _selectedTileBody;
        [SerializeField] private SpriteRenderer _selectTileRender;
        [Space]
        [SerializeField] private MonoPool<TileCellView> _pathPointsPool;
        [SerializeField] private MonoPool<TileCellView> _zonePointsPool;

        [SerializeField] private Color _newPathColor;
        [SerializeField] private Color _oldPathColor;

        [SerializeField] private Color _selectedTileUnWalcableColor;
        [SerializeField] private Color _selectedTileWalcableColor;

        private IReadOnlyList<Vector3Int> _path;
        private int _pointIndex;

        
        public void Clear()
        {
            _pathPointsPool.DisableAll();
            _zonePointsPool.DisableAll();
        }

        public void DrawZone(IReadOnlyList<Vector3Int> zone)
        {
            Clear();
            
            foreach (Vector3Int cell in zone)
            {
                TileCellView tile  =_zonePointsPool.GetNext();
                tile.transform.position = _tilemap.CellAsCenterWorld(cell);
            }
        }
        
        public void DrawLastPath()
        {
            if (_path != null && _path.Count > 0)
            {
                DrawPath(_path, _pointIndex);
            }
        }

        public void DrawPath(IReadOnlyList<Vector3Int> path, int currentPointIndex)
        {
            Clear();

            _path = path;
            _pointIndex = currentPointIndex;
            
            for (int i = 0; i < _path.Count; i++)
            {
                Vector3Int vector3 = path[i];
                TileCellView tile = _pathPointsPool.GetNext();
                tile.transform.position = _tilemap.CellAsCenterWorld(vector3);
                tile.SetColor(i >= _pointIndex ? _newPathColor : _oldPathColor);
            }
        }

      

        public void UpdateSelectTile(Vector3Int position, bool isWalkable)
        {
            _selectedTileBody.position = _tilemap.CellAsCenterWorld(position);
            _selectTileRender.color = isWalkable ? _selectedTileWalcableColor : _selectedTileUnWalcableColor;
        }
    }
}