using System.Collections.Generic;
using System.Linq;
using Essential.Data.BoolMatrix;
using Essential.Extensions;
using UnityEngine;

namespace Code.Game.World
{
    /// <summary>
    /// Physics2D sampling only in enabled BoolMatrix cells.
    /// Matrix is centered on this transform; each cell is CELL_SIZE world units.
    /// </summary>
    public class MatrixTriggerZone : Essential.Mono
    {
        public BoolMatrix Cells => _cells;

        [SerializeField] private EEntityType[] _entityTypes;
        [SerializeField] private BoolMatrix _cells = new(4, 4);
        [SerializeField] private LayerMask _layerMask = ~0;
        [SerializeField] private bool _useTriggers = true;
        [SerializeField, Min(1)] private int _bufferSize = 32;

        private readonly List<Vector2Int> _enabledCells = new();
        private readonly HashSet<int> _seen = new();
        private Collider2D[] _buffer;
        private ContactFilter2D _filter;

        private void Awake()
        {
            _buffer = new Collider2D[_bufferSize];
            _rebuildFilter();
        }

        public int OverlapColliders(List<Collider2D> results)
        {
            return _overlap(results, collectAlive: false, aliveResults: null);
        }

        public int OverlapAlive(List<IWorldEntityView> results)
        {
            results.Clear();
            return _overlap(null, collectAlive: true, aliveResults: results);
        }

        private int _overlap(ICollection<Collider2D> colliderResults, bool collectAlive, ICollection<IWorldEntityView> aliveResults)
        {
            colliderResults?.Clear();
            _ensureBuffer();
            _seen.Clear();
            _cells.GetEnabledCells(_enabledCells);

            Vector2 cellSize = new(VectorExtension.CELL_SIZE, VectorExtension.CELL_SIZE);
            int total = 0;

            for (int i = 0; i < _enabledCells.Count; i++)
            {
                Vector2Int cell = _enabledCells[i];
                Vector2 center = GetCellCenterWorld(cell.x, cell.y);
                int count = Physics2D.OverlapBox(center, cellSize, 0f, _filter, _buffer);

                for (int hit = 0; hit < count; hit++)
                {
                    Collider2D collider = _buffer[hit];
                    if (collider == null)
                    {
                        continue;
                    }

                    if (!_seen.Add(collider.GetEntityId().GetHashCode()))
                    {
                        continue;
                    }

                    if (collectAlive)
                    {
                        if (collider.TryGetComponent(out IWorldEntityView entity) && _entityTypes.Contains(entity.Type))
                        {
                            aliveResults.Add(entity);
                            total++;
                        }
                    }
                    else
                    {
                        colliderResults.Add(collider);
                        total++;
                    }
                }
            }

            return total;
        }

        public Vector2 GetCellCenterWorld(int x, int y)
        {
            Vector2 origin = _getLocalOrigin();
            Vector3 local = new(
                origin.x + (x + 0.5f) * VectorExtension.CELL_SIZE,
                origin.y + (y + 0.5f) * VectorExtension.CELL_SIZE,
                0f);

            return transform.TransformPoint(local);
        }

        /// <summary>Local bottom-left of the whole matrix (centered on transform).</summary>
        private Vector2 _getLocalOrigin()
        {
            return VectorExtension.GetCenteredMatrixOrigin(_cells.Width, _cells.Height);
        }

        private Bounds _getWorldBounds()
        {
            Vector2 origin = _getLocalOrigin();
            Vector3 minLocal = new(origin.x, origin.y, 0f);
            Vector3 maxLocal = new(
                origin.x + _cells.Width * VectorExtension.CELL_SIZE,
                origin.y + _cells.Height * VectorExtension.CELL_SIZE,
                0f);

            Bounds bounds = new(transform.TransformPoint(minLocal), Vector3.zero);
            bounds.Encapsulate(transform.TransformPoint(maxLocal));
            return bounds;
        }

        private void _ensureBuffer()
        {
            if (_buffer == null || _buffer.Length != _bufferSize)
            {
                _buffer = new Collider2D[_bufferSize];
            }

            _rebuildFilter();
        }

        private void _rebuildFilter()
        {
            _filter = new ContactFilter2D
            {
                useTriggers = _useTriggers
            };
            _filter.SetLayerMask(_layerMask);
            _filter.useLayerMask = true;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_bufferSize < 1)
            {
                _bufferSize = 1;
            }

            _rebuildFilter();
        }

        private void OnDrawGizmos()
        {
            if (_cells == null)
            {
                return;
            }

            _cells.GetEnabledCells(_enabledCells);
            Vector3 size = new(VectorExtension.CELL_SIZE, VectorExtension.CELL_SIZE, 0f);

            Gizmos.color = ColorExtension.OBSTACLE_GIZMO_COLOR;
            for (int i = 0; i < _enabledCells.Count; i++)
            {
                Vector2Int cell = _enabledCells[i];
                Gizmos.DrawCube(GetCellCenterWorld(cell.x, cell.y), size);
            }

            Bounds bounds = _getWorldBounds();
            Gizmos.color = new Color(1f, 1f, 1f, 0.25f);
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
#endif
    }
}
