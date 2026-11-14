using System.Collections.Generic;
using Code.Pathfinding;
using Essential;
using Essential.Data.BoolMatrix;
using Essential.Extensions;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Code.Game.World
{
    public sealed class MatrixZone : WorldMono
    {
        [SerializeField] private BoolMatrix _matrix = new(4, 4);
        [SerializeField] private Vector3Int _originCellPosition;
        [SerializeField] private TilemapView _tilemapView;


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

        public void UpdateOriginCell()
        {
            if (_tilemapView == null)
            {
                Debug.LogError("tilemap view is null");
                
                return;
            }
            
            Vector3Int localCellPosition = _tilemapView.WorldAsCell(transform.position); 
            
            _originCellPosition = localCellPosition + _matrix.GetCenteredMatrixOriginCell();
        }

        private Vector3Int _getCell(int indexX, int indexY)
        {
            return _originCellPosition + new Vector3Int(indexX, indexY);
        }

#if UNITY_EDITOR

        
        protected override void e_onSpanToTilemap(TilemapView tilemap, Vector3Int cell)
        {
            _tilemapView = tilemap;
            _originCellPosition = cell + _matrix.GetCenteredMatrixOriginCell();
            
            EditorUtility.SetDirty(this);
        }

        private void OnDrawGizmos()
        {
            if (_matrix == null || _tilemapView == null)
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
                Gizmos.DrawCube(_tilemapView.CellAsCenterWorld(cell), cellSize);
            }

            //bounds
            Vector3Int minCell = _originCellPosition;
            Vector3Int maxCellExclusive = _originCellPosition + new Vector3Int(_matrix.Width, _matrix.Height, 0);
            Vector3 min = _tilemapView.CellAsWorld(minCell);
            Vector3 max = _tilemapView.CellAsWorld(maxCellExclusive);
            Vector3 center = (min + max) * 0.5f;
            Vector3 size = max - min;
            size.z = 0f;
            Gizmos.color = new Color(1f, 1f, 1f, 0.25f);
            Gizmos.DrawWireCube(center, size);
        }
        
#endif
    }
}
