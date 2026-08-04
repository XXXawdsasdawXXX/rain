using System;
using Code.Pathfinding;
using Core.Input;
using JetBrains.Annotations;
using UnityEngine;
using VContainer.Unity;

namespace Code.Game.Characters
{
    [UsedImplicitly]
    public class PartyPathDrawer : IStartable, IDisposable, ITickable
    {
        private readonly IPathfindingDrawer _pathfindingDrawer;
        private readonly IPlayerPartyMovement _playerPartyMovement;
        private readonly IInputService _input;
        private readonly PathfindingTilemapView _pathfindingTilemapView;

        private Vector3 _lastCellPosition;
        
        public PartyPathDrawer(
            IPathfindingDrawer pathfindingDrawer, 
            IPlayerPartyMovement playerPartyMovement,
            IInputService input,
            PathfindingTilemapView pathfindingTilemapView)
        {
            _pathfindingDrawer = pathfindingDrawer;
            _playerPartyMovement = playerPartyMovement;
            _input = input;
            _pathfindingTilemapView = pathfindingTilemapView;
        }
        
        public void Start()
        {
            
        }

        public void Dispose()
        {
          
        }

        public void Tick()
        {
            Vector3 currentCell = _pathfindingTilemapView.RoundWorldPosition(_input.MouseWorldPosition);

            if (currentCell != _lastCellPosition)
            {
                _pathfindingDrawer.UpdateSelectTile(currentCell,true);//TODO SET IS WALKABLE
                _lastCellPosition = currentCell;
            }
        }
    }
}