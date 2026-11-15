using System;
using UnityEngine.Tilemaps;

namespace Code.Game.Pathfinding
{
    [Serializable]
    public class TileData
    {
        public bool IsWalkable;
        public int Weight;
        public TileBase[] Tiles;
    }
}