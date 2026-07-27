using UnityEngine;

namespace Code.Pathfinding
{
    public static class PathfindingCollisionChecker
    {
        private static readonly Collider2D[] OverlapBuffer = new Collider2D[16];


        public static bool IsBlocked(Vector3 worldPosition, PathfindingAgentSettings settings)
        {
            if (settings.CheckRadius <= 0f)
            {
                return false;
            }

            ContactFilter2D filter = new();
            filter.SetLayerMask(settings.ObstacleLayers);
            filter.useTriggers = true;

            int count = Physics2D.OverlapCircle(
                new Vector2(worldPosition.x, worldPosition.y),
                settings.CheckRadius,
                filter,
                OverlapBuffer);

            for (int i = 0; i < count; i++)
            {
                Collider2D collider = OverlapBuffer[i];
                
                if (collider == null || collider == settings.IgnoreCollider)
                {
                    continue;
                }

                if (!collider.TryGetComponent(out IPathfindingObstacle obstacle))
                {
                    return true;
                }
                
                if (!obstacle.BlocksPathfinding)
                {
                    continue;
                }

                return true;
            }

            return false;
        }
    }
}
