using System.Collections.Generic;
using Essential;
using TriInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using VContainer.Unity;

namespace Code.Pathfinding
{
    public class PathfindingTilemapView : Essential.Mono, IStartable
    {
        [SerializeField] private Tilemap _tilemapBase;

        [SerializeField, Group("gizmo"), GUIColor(0.75f, 0.75f, 0.3f, 0.9f)]
        private bool _isDrawGizmo;
        
        [SerializeField, Group("gizmo"), GUIColor(0.75f, 0.75f, 0.3f, 0.9f)]
        private Color _gizmoColor = new Color(1, 1, 1, 0.5f);

        [SerializeField, Group("gizmo"), GUIColor(0.75f, 0.75f, 0.3f, 0.9f)]
        private Font _font;
        
        [SerializeField, Group("gizmo"), GUIColor(0.75f, 0.75f, 0.3f, 0.9f)]
        private int _fontSize = 20;

        
        private WeightTilesConfiguration _weightTiles;
        private Dictionary<Vector3Int, TileData> _mapTiles;
        
        
        [Inject]
        public void Construct(WeightTilesConfiguration weightTiles)
        {
            _weightTiles = weightTiles;
        }

        public void Start()
        {
            _bakeMapTiles();
        }

        public TileData GetCellData(Vector3Int cellPosition)
        {
            _ensureMapBaked();
            return _mapTiles.TryGetValue(cellPosition, out TileData data)
                ? data
                : _weightTiles.LockTiles;
        }

        public bool HasBakedCell(Vector3Int cellPosition)
        {
            _ensureMapBaked();
            return _mapTiles.ContainsKey(cellPosition);
        }

        public Vector3Int WordToCell(Vector3 worldPosition)
        {
            return _tilemapBase.WorldToCell(worldPosition);
        }

        public Vector3 CellToWorlds(Vector3Int cellPosition)
        {
            return _tilemapBase.GetCellCenterWorld(cellPosition);
        }

        public int GetTileWeight(Vector3Int cellPosition)
        {
            return GetCellData(cellPosition).Weight;
        }

        private void _ensureMapBaked()
        {
            if (_mapTiles != null)
            {
                return;
            }

            _bakeMapTiles();
        }

        private void _bakeMapTiles()
        {
            _mapTiles = new Dictionary<Vector3Int, TileData>();
            BoundsInt bounds = _tilemapBase.cellBounds;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cell = new Vector3Int(x, y, bounds.z);

                    if (!_tilemapBase.HasTile(cell))
                    {
                        continue;
                    }

                    TileBase tile = _tilemapBase.GetTile(cell);
                    if (tile == null)
                    {
                        continue;
                    }

                    _mapTiles[cell] = _resolveTileData(tile);
                }
            }

            Log.Info(this, $"[BakeMapTiles] baked {_mapTiles.Count} cells");
        }

        private TileData _resolveTileData(TileBase tile)
        {
            foreach (TileData tileData in _weightTiles.TilesData)
            {
                foreach (TileBase tileRef in tileData.Tiles)
                {
                    if (tileRef == tile)
                    {
                        return tileData;
                    }
                }
            }

            return _weightTiles.LockTiles;
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_isDrawGizmo || _tilemapBase == null)
            {
                return;
            }
            
            BoundsInt bounds = _tilemapBase.cellBounds;
            Vector3 samplePosition = _tilemapBase.GetCellCenterWorld(new Vector3Int(bounds.xMin, bounds.yMin, bounds.z));
            float handleSize = Mathf.Max(HandleUtility.GetHandleSize(samplePosition), 0.0001f);
            float cellSize = Mathf.Max(_tilemapBase.cellSize.x, _tilemapBase.cellSize.y);
            int fontSize = Mathf.Max(1, Mathf.RoundToInt(_fontSize * cellSize / handleSize));

            GUIStyle labelStyle = new()
            {
                alignment = TextAnchor.MiddleCenter,
                font = _font,
                fontSize = fontSize,
                fontStyle = FontStyle.Italic,
                normal = { textColor = _gizmoColor },
            };

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cell = new(x, y, bounds.z);
                    Handles.Label(CellToWorlds(cell), cell.ToString(), labelStyle);
                }
            }
        }
#endif
    }
}