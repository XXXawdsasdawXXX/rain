using System;
using UnityEngine.Tilemaps;

namespace Code.Pathfinding
{
    [Serializable]
    public class TileData
    {
        public bool IsWalkable;
        public int Weight;
        public TileBase[] Tiles;
    }
}