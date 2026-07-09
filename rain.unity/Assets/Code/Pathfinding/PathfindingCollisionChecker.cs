using UnityEngine;

namespace Code.Pathfinding
{
    public sealed class PathfindingCollisionChecker
    {
        private static readonly Collider2D[] OverlapBuffer = new Collider2D[16];

        private readonly PathfindingTilemapView _tilemap;

        public PathfindingCollisionChecker(PathfindingTilemapView tilemap)
        {
            _tilemap = tilemap;
        }

        public bool IsBlocked(Vector3Int cell, PathfindingAgentSettings settings)
        {
            if (settings.CheckRadius <= 0f)
            {
                return false;
            }

            Vector3 world = _tilemap.CellToWorlds(cell);
            var filter = new ContactFilter2D();
            filter.SetLayerMask(settings.ObstacleLayers);
            filter.useTriggers = true;

            int count = Physics2D.OverlapCircle(
                new Vector2(world.x, world.y),
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

                if (collider.TryGetComponent<IPathfindingObstacle>(out IPathfindingObstacle obstacle))
                {
                    if (!obstacle.BlocksPathfinding)
                    {
                        continue;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
