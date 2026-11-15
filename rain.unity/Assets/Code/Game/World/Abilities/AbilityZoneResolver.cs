using System.Collections.Generic;
using Essential.Data.BoolMatrix;
using UnityEngine;

namespace Code.Game.World.Abilities
{
    public class AbilityZoneResolver
    {
        public static IReadOnlyList<Vector3Int> GetCells(BoolMatrix zone, Vector3Int origin)
        {
            List<Vector2Int> indexes = new();
            zone.GetEnableIndexes(indexes);
            Vector3Int zoneOrigin = zone.GetCenteredMatrixOriginCell();
            
            List<Vector3Int> worldCells = new();
            foreach (Vector2Int index in indexes)
            {
                Vector3Int worldCell = origin + zoneOrigin + new Vector3Int(index.x, index.y);
                worldCells.Add(worldCell);
            }

            return worldCells;
        }
    }
}