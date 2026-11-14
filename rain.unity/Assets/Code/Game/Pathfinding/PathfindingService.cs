using System.Collections.Generic;
using System.Linq;
using Code.Game.World.Turns;
using Essential;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Pathfinding
{
    [UsedImplicitly]
    public sealed class PathfindingService : IPathfindingService
    {
        public ITilemapView Tilemap => _tilemap;

        private readonly List<PathfindingNode> _openNodes = new();
        private readonly HashSet<Vector3Int> _closedCells = new();
        private readonly Dictionary<Vector3Int, PathfindingNode> _openLookup = new();
        private readonly List<Vector3Int> _neighborCells = new();
        private readonly List<Vector3Int> _path = new();

        private readonly ITilemapView _tilemap;

        private Vector3Int _endCellPosition;
        private readonly IEntityRegistry _entityRegistry;


        public PathfindingService(ITilemapView tilemap, IEntityRegistry entityRegistry)
        {
            _tilemap = tilemap;
            _entityRegistry = entityRegistry;
        }

        public IReadOnlyList<Vector3Int> FindPath(Vector3 startPosition, Vector3 endPosition)
        {
            Vector3Int start = _tilemap.WorldAsCell(startPosition);
            Vector3Int end = _tilemap.WorldAsCell(endPosition);
            return FindPath(start, end);
        }

        public IReadOnlyList<Vector3Int> FindPath(Vector3Int startPosition, Vector3Int endPosition)
        {
            List<Vector3Int> path = new();
            
            _path.Clear();

            _endCellPosition = endPosition;

            if (!_isWalkable(startPosition))
            {
                Log.Info(this,
                    $"[FindPath] start cell {startPosition} is not walkable (baked={_tilemap.HasBakedCell(startPosition)})");
                return path;
            }

            if (!_isWalkable(_endCellPosition))
            {
                Log.Info(this, $"[FindPath] end cell {_endCellPosition} is not walkable");
                return path;
            }

            if (startPosition == _endCellPosition)
            {
                return path;
            }

            PathfindingNode endNode = _runAStar(startPosition);
            if (endNode == null)
            {
                Log.Info(this, $"[FindPath] no path from {startPosition} to {_endCellPosition}");
                return path;
            }

            _reconstructPath(endNode);

            for (int i = 0; i < _path.Count; i++)
            {
                Vector3Int cell = _path[i];
                path.Add(cell);
            }

            Log.Info(this, $"[FindPath] start={startPosition}, end={_endCellPosition}, steps={_path.Count - 1}");
            
            return path;
        }

        public bool IsWalkable(Vector3Int cellPosition)
        {
            return _isWalkable(cellPosition);
        }
        
        private PathfindingNode _runAStar(Vector3Int startCell)
        {
            _openNodes.Clear();
            _closedCells.Clear();
            _openLookup.Clear();

            PathfindingNode startNode = new()
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

                PathfindingNode neighborNode = new()
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
            
            return _entityRegistry.IsEmptyCell(cell);
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
}
