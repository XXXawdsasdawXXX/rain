using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace Code.Pathfinding
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "Game/Configuration/TilesWeight")]
    public class WeightTilesConfiguration : ScriptableObject
    {
        [field: SerializeField, GUIColor(0.9f, 0.35f, 0.5f, 0.95f)] 
        public TileData LockTiles { get; private set; }
        
        [field: SerializeField] public List<TileData> TilesData { get; private set; }
    }
}