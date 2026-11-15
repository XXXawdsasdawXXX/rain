using UnityEngine;

namespace Code.Game.Pathfinding
{
    public class PathfindingNode
    {
        public Vector3Int CellPosition;
        public int GCost;
        public int HCost;
        public int TileWeight;
        public int FCost => GCost + HCost;
        public PathfindingNode Parent;
    }
}