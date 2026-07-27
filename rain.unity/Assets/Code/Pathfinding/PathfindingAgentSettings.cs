using System;
using UnityEngine;

namespace Code.Pathfinding
{
    [Serializable]
    public struct PathfindingAgentSettings
    {
        public LayerMask ObstacleLayers { get; private set; }
        public float CheckRadius { get; private set; }
        public Collider2D IgnoreCollider { get; private set; }
        
        public static PathfindingAgentSettings Default => new()
        {
            ObstacleLayers = Physics2D.AllLayers,
            CheckRadius = 0.4f,
            IgnoreCollider = null
        };
    }
}
