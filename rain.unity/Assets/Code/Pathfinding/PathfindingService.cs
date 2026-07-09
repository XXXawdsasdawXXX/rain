using System.Collections.Generic;
using System.Linq;
using Essential;
using UnityEngine;

namespace Code.Pathfinding
{
    public interface IPathfindingService
    {
        bool HasActivePath { get; }
        void FindPath(Vector3 startPosition, Vector3 endPosition, PathfindingAgentSettings settings);
        Vector3 MoveToNextWorldPosition();
    }

    public class PathfindingService : IPathfindingService
    {
        private readonly List<PathfindingNode> _openNodes = new();
        private readonly HashSet<Vector3Int> _closedCells = new();
        private readonly Dictionary<Vector3Int, PathfindingNode> _openLookup = new();
        private readonly List<Vector3Int> _neighborCells = new();
        private readonly List<Vector3Int> _path = new();

        private readonly PathfindingTilemapView _tilemap;
        private readonly PathfindingCollisionChecker _collisionChecker;

        private PathfindingAgentSettings _agentSettings;
        private Vector3Int _endCellPosition;
        private int _pathIndex;

        public bool HasActivePath { get; private set; }

        public PathfindingService(PathfindingTilemapView tilemap, PathfindingCollisionChecker collisionChecker)
        {
            _tilemap = tilemap;
            _collisionChecker = collisionChecker;
        }

        public void FindPath(Vector3 startPosition, Vector3 endPosition, PathfindingAgentSettings settings)
        {
            _path.Clear();
            _pathIndex = 0;
            HasActivePath = false;
            _agentSettings = settings;

            Vector3Int startCell = _tilemap.WordToCell(startPosition);
            _endCellPosition = _tilemap.WordToCell(endPosition);

            if (!_isWalkable(startCell))
            {
                Log.Info(this,
                    $"[FindPath] start cell {startCell} is not walkable (baked={_tilemap.HasBakedCell(startCell)})");
                return;
            }

            if (!_isWalkable(_endCellPosition))
            {
                Log.Info(this, $"[FindPath] end cell {_endCellPosition} is not walkable");
                return;
            }

            if (startCell == _endCellPosition)
            {
                return;
            }

            PathfindingNode endNode = _runAStar(startCell);
            if (endNode == null)
            {
                Log.Info(this, $"[FindPath] no path from {startCell} to {_endCellPosition}");
                return;
            }

            _reconstructPath(endNode);
            _pathIndex = 1;
            HasActivePath = _path.Count > 1;

            Log.Info(this, $"[FindPath] start={startCell}, end={_endCellPosition}, steps={_path.Count - 1}");
        }

        public Vector3 MoveToNextWorldPosition()
        {
            if (!HasActivePath || _pathIndex >= _path.Count)
            {
                HasActivePath = false;
                Vector3Int cell = _path.Count > 0 ? _path[^1] : Vector3Int.zero;
                return _tilemap.CellToWorlds(cell);
            }

            Vector3Int nextCell = _path[_pathIndex];

            if (_collisionChecker.IsBlocked(nextCell, _agentSettings))
            {
                Vector3Int currentCell = _path[_pathIndex - 1];
                if (!_tryRepath(currentCell))
                {
                    Log.Info(this, "[MoveToNextWorldPosition] blocked and repath failed");
                    HasActivePath = false;
                    return _tilemap.CellToWorlds(currentCell);
                }

                nextCell = _path[_pathIndex];
            }

            _pathIndex++;
            HasActivePath = _pathIndex < _path.Count;

            Log.Info(this, $"[MoveToNextWorldPosition] step to {nextCell}");

            return _tilemap.CellToWorlds(nextCell);
        }

        private bool _tryRepath(Vector3Int fromCell)
        {
            PathfindingNode endNode = _runAStar(fromCell);
            if (endNode == null)
            {
                return false;
            }

            _reconstructPath(endNode);
            _pathIndex = 1;
            HasActivePath = _path.Count > 1;
            return HasActivePath;
        }

        private PathfindingNode _runAStar(Vector3Int startCell)
        {
            _openNodes.Clear();
            _closedCells.Clear();
            _openLookup.Clear();

            var startNode = new PathfindingNode
            {
                CellPosition = startCell,
                GCost = 0,
                HCost = _calculateHeuristic(startCell),
                TileWeight = _tilemap.GetCellData(startCell).Weight,
                Parent = null
            };

            _openNodes.Add(startNode);
            _openLookup[startCell] = startNode;

            while (_openNodes.Count > 0)
            {
                PathfindingNode current = _pickLowestFNode();
                _openNodes.Remove(current);
                _openLookup.Remove(current.CellPosition);

                if (current.CellPosition == _endCellPosition)
                {
                    return current;
                }

                _closedCells.Add(current.CellPosition);
                _expandNode(current);
            }

            return null;
        }

        private void _expandNode(PathfindingNode current)
        {
            _fillNeighborCells(current.CellPosition);

            foreach (Vector3Int neighborCell in _neighborCells)
            {
                if (_closedCells.Contains(neighborCell))
                {
                    continue;
                }

                Vector3Int delta = neighborCell - current.CellPosition;
                int moveCost = PathfindingTools.NeighborOffsets[delta];
                TileData data = _tilemap.GetCellData(neighborCell);
                int newG = current.GCost + moveCost + data.Weight;
                int newH = _calculateHeuristic(neighborCell);

                if (_openLookup.TryGetValue(neighborCell, out PathfindingNode existing))
                {
                    if (newG >= existing.GCost)
                    {
                        continue;
                    }

                    existing.GCost = newG;
                    existing.HCost = newH;
                    existing.TileWeight = data.Weight;
                    existing.Parent = current;
                    continue;
                }

                var neighborNode = new PathfindingNode
                {
                    CellPosition = neighborCell,
                    GCost = newG,
                    HCost = newH,
                    TileWeight = data.Weight,
                    Parent = current
                };

                _openNodes.Add(neighborNode);
                _openLookup[neighborCell] = neighborNode;
            }
        }

        private void _fillNeighborCells(Vector3Int current)
        {
            _neighborCells.Clear();

            foreach ((Vector3Int offset, int _) in PathfindingTools.NeighborOffsets)
            {
                Vector3Int neighbor = current + offset;

                if (!_isWalkable(neighbor))
                {
                    continue;
                }

                _neighborCells.Add(neighbor);
            }
        }

        private bool _isWalkable(Vector3Int cell)
        {
            if (!_tilemap.GetCellData(cell).IsWalkable)
            {
                return false;
            }

            return !_collisionChecker.IsBlocked(cell, _agentSettings);
        }

        private PathfindingNode _pickLowestFNode()
        {
            return _openNodes
                .OrderBy(n => n.FCost)
                .ThenBy(n => n.HCost)
                .First();
        }

        private void _reconstructPath(PathfindingNode endNode)
        {
            _path.Clear();

            PathfindingNode current = endNode;
            while (current != null)
            {
                _path.Add(current.CellPosition);
                current = current.Parent;
            }

            _path.Reverse();
        }

        private int _calculateHeuristic(Vector3Int cell)
        {
            int dx = Mathf.Abs(cell.x - _endCellPosition.x);
            int dy = Mathf.Abs(cell.y - _endCellPosition.y);

            return 10 * (dx + dy) + (14 - 2 * 10) * Mathf.Min(dx, dy);
        }
    }

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
