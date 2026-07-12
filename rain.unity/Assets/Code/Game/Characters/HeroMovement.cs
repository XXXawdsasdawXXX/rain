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

        private IPathfindingService _pathfinding;
        private IInputService _input;
        private PathfindingAgentSettings _agentSettings;

        
        [Inject]
        public void Construct(IPathfindingService pathfinding, IInputService input)
        {
            _pathfinding = pathfinding;
            _input = input;
        }

        private void Awake()
        {
            _agentSettings = new PathfindingAgentSettings
            {
                ObstacleLayers = _obstacleLayers,
                CheckRadius = _checkRadius,
                IgnoreCollider = _collider
            };
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
            _pathfinding.FindPath(_character.position, _input.MouseWorldPosition, _agentSettings);
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
