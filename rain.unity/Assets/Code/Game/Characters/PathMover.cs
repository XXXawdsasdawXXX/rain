using System;
using Code.Pathfinding;
using Core.Input;
using UnityEngine;
using VContainer;

namespace Code.Game.Characters
{
    public class PathMover : Essential.Mono
    {
        [SerializeField] private Transform _character;

        private IPathfindingService _pathfinding;
        private IInputService _input;

        [Inject]
        public void Construct(IPathfindingService pathfinding, IInputService input)
        {
            _pathfinding = pathfinding;
            _input = input;
        }

        private void OnEnable()
        {
            _input.ActionStarted += _onInputActionStarted;
            _input.ActionEnded += _onInputActionEnded;
        }

        private void OnDisable()
        {
            _input.ActionStarted -= _onInputActionStarted;
            _input.ActionEnded -= _onInputActionEnded;
        }

        private void _onInputActionStarted(EInputAction action)
        {
            if (action == EInputAction.LeftClick)
            {
                _pathfinding.FindPath(_character.position, _input.MouseWorldPosition);
            }
        }

        private void _onInputActionEnded(EInputAction action)
        {
            if (action != EInputAction.RightClick || !_pathfinding.HasActivePath)
            {
                return;
            }

            _character.position = _pathfinding.MoveToNextWorldPosition();
        }
    }
}
