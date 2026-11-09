using UnityEngine;

namespace Code.Pathfinding
{
    public interface ITilemapView
    {
        public Vector3Int WorldAsCell(Vector3 worldPosition);
        public Vector3 CellAsCenterWorld(Vector3Int cellPosition);
        bool HasBakedCell(Vector3Int cell);
        TileData GetCellData(Vector3Int cell);
    }
}