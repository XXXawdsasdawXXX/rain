using System;
using System.Collections.Generic;
using Code.Pathfinding;
using Core.Input;
using JetBrains.Annotations;
using UnityEngine;
using VContainer.Unity;

namespace Code.Game.Characters
{
    [UsedImplicitly]
    public class PlayerPathDrawer : IStartable, IDisposable, ITickable
    {
        private readonly IPathfindingDrawer _pathfindingDrawer;
        private readonly IPlayerPartyMovement _playerPartyMovement;
        private readonly IInputService _input;
        private readonly PathfindingTilemapView _pathfindingTilemapView;
        private readonly IPathfindingService _pathfindingService;

        private Vector3Int _lastCellPosition;
        
        public PlayerPathDrawer(
            IPathfindingDrawer pathfindingDrawer, 
            IPlayerPartyMovement playerPartyMovement,
            IInputService input,
            PathfindingTilemapView pathfindingTilemapView,
            IPathfindingService pathfindingService)
        {
            _pathfindingDrawer = pathfindingDrawer;
            _playerPartyMovement = playerPartyMovement;
            _input = input;
            _pathfindingTilemapView = pathfindingTilemapView;
            _pathfindingService = pathfindingService;
        }
        
        public void Start()
        {
            _playerPartyMovement.PathUpdated += _redrawPath;
        }

        public void Dispose()
        {
            _playerPartyMovement.PathUpdated -= _redrawPath;
        }

        public void Tick()
        {
            Vector3Int currentCell = _pathfindingTilemapView.WordToCell(_input.MouseWorldPosition);

            if (currentCell != _lastCellPosition)
            {
                Vector3 viewPosition = _pathfindingTilemapView.RoundWorldPosition(_input.MouseWorldPosition);
                _pathfindingDrawer.UpdateSelectTile(viewPosition, _pathfindingService.IsWalkable(currentCell));
                _lastCellPosition = currentCell;;
            }
        }

        private void _redrawPath(CharacterView _, List<Vector3> path, int currentPointIndex)
        {
            _pathfindingDrawer.DrawPath(path, currentPointIndex);
        }
    }
}