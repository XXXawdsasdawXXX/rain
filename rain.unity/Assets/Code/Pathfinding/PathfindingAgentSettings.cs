using UnityEngine;

namespace Code.Pathfinding
{
    public struct PathfindingAgentSettings
    {
        public LayerMask ObstacleLayers;
        public float CheckRadius;
        public Collider2D IgnoreCollider;

        public static PathfindingAgentSettings Default => new()
        {
            ObstacleLayers = Physics2D.AllLayers,
            CheckRadius = 0.4f,
            IgnoreCollider = null
        };
    }
}
