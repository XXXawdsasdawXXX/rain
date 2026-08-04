using System;
using System.Collections.Generic;
using Code.Game.Characters.Hero;
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
        private readonly IPlayerPartyProvider _playerPartyProvider;

        public PlayerPathDrawer(
            IPathfindingDrawer pathfindingDrawer, 
            IPlayerPartyMovement playerPartyMovement,
            IInputService input,
            PathfindingTilemapView pathfindingTilemapView,
            IPathfindingService pathfindingService,
            IPlayerPartyProvider playerPartyProvider)
        {
            _pathfindingDrawer = pathfindingDrawer;
            _playerPartyMovement = playerPartyMovement;
            _input = input;
            _pathfindingTilemapView = pathfindingTilemapView;
            _pathfindingService = pathfindingService;
            _playerPartyProvider = playerPartyProvider;
        }
        
        public void Start()
        {
            _playerPartyMovement.PathUpdated += _redrawPath;
            _playerPartyProvider.CharacterSelected += _updatePathView;
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
                bool isWalkable = _pathfindingService.IsWalkable(
                    viewPosition,
                    _playerPartyProvider.CurrentCharacter.AgentSettings);
                
                _pathfindingDrawer.UpdateSelectTile(viewPosition, isWalkable);
                _lastCellPosition = currentCell;
            }
        }

        private void _updatePathView(CharacterView hero)
        {
            IPathfindingFollower follower = _playerPartyMovement.GetFollower(hero);
            _pathfindingDrawer.DrawPath(follower.Path, follower.PointIndex);    
        }

        private void _redrawPath(CharacterView _, List<Vector3> path, int currentPointIndex)
        {
            _pathfindingDrawer.DrawPath(path, currentPointIndex);
        }
    }
}