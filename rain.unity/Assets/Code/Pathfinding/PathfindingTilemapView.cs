using System.Collections.Generic;
using Essential;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using VContainer.Unity;

namespace Code.Pathfinding
{
    public class PathfindingTilemapView : Essential.Mono, IStartable
    {
        [SerializeField] private Tilemap _tilemap;

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
            return _tilemap.WorldToCell(worldPosition);
        }

        public Vector3 CellToWorlds(Vector3Int cellPosition)
        {
            return _tilemap.GetCellCenterWorld(cellPosition);
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
            BoundsInt bounds = _tilemap.cellBounds;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cell = new Vector3Int(x, y, bounds.z);

                    if (!_tilemap.HasTile(cell))
                    {
                        continue;
                    }

                    TileBase tile = _tilemap.GetTile(cell);
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
    }
}
