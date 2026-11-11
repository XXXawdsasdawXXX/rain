using System.Collections.Generic;
using Code.Pathfinding;
using Essential;
using Essential.Data.BoolMatrix;
using Essential.Extensions;
using UnityEngine;

namespace Code.Game.World
{
    public sealed class MatrixTriggerZone : WorldMono
    {
        [SerializeField] private BoolMatrix _matrix = new(4, 4);
        [SerializeField] private Vector3Int _originCellPosition;
        

        public IReadOnlyList<Vector3Int> GetCells()
        {
            List<Vector3Int> cells = new();
            List<Vector2Int> matrix = new();
          
            _matrix.GetEnableIndexes(matrix);

            foreach (Vector2Int index in matrix)
            {
                cells.Add(_getCell(index.x, index.y));
            }

            return cells;
        }

        private Vector3Int _getCell(int indexX, int indexY)
        {
            return _originCellPosition + new Vector3Int(indexX, indexY);
        }

#if UNITY_EDITOR

        [SerializeField] private TilemapView e_tilemapView;
        
     
        protected override void onSpanToTilemap(TilemapView tilemap)
        {
            e_tilemapView = tilemap;
            
            Vector3Int localCellPosition = tilemap.WorldAsCell(transform.position); 
            
            _originCellPosition = localCellPosition + _matrix.GetCenteredMatrixOriginCell();
        }

        private void OnDrawGizmos()
        {
            if (_matrix == null || e_tilemapView == null)
            {
                return;
            }

            //cells
            IReadOnlyList<Vector3Int> cells = GetCells();
            Vector3 cellSize = new(ITilemapView.CELL_SIZE, ITilemapView.CELL_SIZE, 0f);
            Gizmos.color = Colors.GIZMO_OBSTACLE_COLOR;
            for (int i = 0; i < cells.Count; i++)
            {
                Vector3Int cell = cells[i];
                Gizmos.DrawCube(e_tilemapView.CellAsCenterWorld(cell), cellSize);
            }

            //bounds
            Vector3Int minCell = _originCellPosition;
            Vector3Int maxCellExclusive = _originCellPosition + new Vector3Int(_matrix.Width, _matrix.Height, 0);
            Vector3 min = e_tilemapView.CellAsWorld(minCell);
            Vector3 max = e_tilemapView.CellAsWorld(maxCellExclusive);
            Vector3 center = (min + max) * 0.5f;
            Vector3 size = max - min;
            size.z = 0f;
            Gizmos.color = new Color(1f, 1f, 1f, 0.25f);
            Gizmos.DrawWireCube(center, size);
        }
        
#endif
    }
}
