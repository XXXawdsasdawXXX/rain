using System.Collections.Generic;
using Code.Pathfinding;
using Core.Input;
using UnityEngine;
using VContainer;

namespace Code.Game.Characters
{
    public class HeroMovement : Essential.Mono
    {
        [SerializeField] private Transform _character;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private LayerMask _obstacleLayers;
        [SerializeField] private float _checkRadius = 0.4f;

        private IInputService _input;
        private PathfindingAgentSettings _agentSettings;
        private IPathfindingFollower _follower;
        private IPathfindingService _pathfinding;

        
        [Inject]
        public void Construct(IPathfindingService pathfinding, IInputService input)
        {
            _agentSettings = new PathfindingAgentSettings
            {
                ObstacleLayers = _obstacleLayers,
                CheckRadius = _checkRadius,
                IgnoreCollider = _collider
            };
            
            _follower = new CharacterPathfindingFollower();
            _input = input;

            _pathfinding = pathfinding;
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
            if (action != EInputAction.LeftClick)
            {
                return;
            }

            _agentSettings.IgnoreCollider = _collider;
            
            IReadOnlyList<Vector3> path = _pathfinding.FindPath(
                _character.position, _input.MouseWorldPosition, _agentSettings);

            _follower.SetPath(path);
        }

        private void _onInputActionEnded(EInputAction action)
        {
            if (action != EInputAction.RightClick || !_follower.HasActivePath)
            {
                return;
            }

            if (_follower.TryMoveToNextWorldPosition(out Vector3 nextPosition))
            {
                _character.position = nextPosition;
            }
        }
    }
}
