using System;
using Code.Game.Characters.Hero;
using Code.Game.StateMachine;
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
        private readonly IInputService _input;
        private readonly PathfindingTilemapView _pathfindingTilemapView;
        private readonly IPathfindingService _pathfindingService;
        private readonly IPlayerPartyProvider _playerPartyProvider;
        private readonly IGamePhaseService _gamePhase;

        private Vector3Int _lastCellPosition;
        private IPathfindingFollower _currentFollower;


        public PlayerPathDrawer(
            IPathfindingDrawer pathfindingDrawer, 
            IInputService input,
            PathfindingTilemapView pathfindingTilemapView,
            IPathfindingService pathfindingService,
            IPlayerPartyProvider playerPartyProvider,
            IGamePhaseService gamePhase)
        {
            _gamePhase = gamePhase;
            _pathfindingDrawer = pathfindingDrawer;
            _input = input;
            _pathfindingTilemapView = pathfindingTilemapView;
            _pathfindingService = pathfindingService;
            _playerPartyProvider = playerPartyProvider;
        }
        
        public void Start()
        {
            _playerPartyProvider.HeroSelected += _onHeroSelected;
        }

        public void Dispose()
        {
            _playerPartyProvider.HeroSelected += _onHeroSelected;
        }

        public void Tick()
        {
            if (_gamePhase.Phase is EGamePhase.Paused)
            {
                return;
            }
            
            Vector3Int currentCell = _pathfindingTilemapView.WordToCell(_input.MouseWorldPosition);

            if (currentCell != _lastCellPosition)
            {
                Vector3 viewPosition = _pathfindingTilemapView.RoundWorldPosition(_input.MouseWorldPosition);
                bool isWalkable = _pathfindingService.IsWalkable(
                    viewPosition,
                    _playerPartyProvider.CurrentCharacter.Render.AgentSettings);
                
                _pathfindingDrawer.UpdateSelectTile(viewPosition, isWalkable);
                _lastCellPosition = currentCell;
            }
        }

        private void _onHeroSelected(CharacterWorldActor hero)
        {
            if (_currentFollower != null)
            {
                _currentFollower.Updated -= _redrawPath;
            }
            
            _currentFollower = hero.PathfindingFollower;
            
            _pathfindingDrawer.DrawPath(_currentFollower.Path, _currentFollower.PointIndex);

            _currentFollower.Updated += _redrawPath;
        }

        private void _redrawPath()
        {
            _pathfindingDrawer.DrawPath(_currentFollower.Path, _currentFollower.PointIndex);
        }
    }
}