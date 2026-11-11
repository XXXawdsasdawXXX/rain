using UnityEngine;

namespace Code.Pathfinding
{
    public interface ITilemapView
    {
        public static float CELL_SIZE = 0.64f;
        
        public Vector3Int WorldAsCell(Vector3 worldPosition);
        public Vector3 CellAsCenterWorld(Vector3Int cellPosition);
        public Vector3 CellAsWorld(Vector3Int cellPosition);
        bool HasBakedCell(Vector3Int cell);
        TileData GetCellData(Vector3Int cell);
    }
}