using System;
using System.Collections.Generic;
using Code.Game.Characters.Hero;
using Code.Pathfinding;
using Core.Input;
using Essential;
using JetBrains.Annotations;
using UnityEngine;
using VContainer.Unity;

namespace Code.Game.Characters
{
    public interface IPlayerPartyMovement
    {
        event Action<CharacterView, List<Vector3>, int> PathUpdated;
    }
    
    [UsedImplicitly]
    public class PlayerPartyMovement : IPlayerPartyMovement, IStartable, IDisposable
    { 
        public event Action<CharacterView, List<Vector3>, int> PathUpdated;

        private readonly IPathfindingService _pathfinding;
        private readonly IInputService _input;
        private readonly IPlayerPartyProvider _playerPartyProvider;
        
        private readonly Dictionary<CharacterView, IPathfindingFollower> _followers = new();

        
        public PlayerPartyMovement(
            IPlayerPartyProvider playerPartyProvider, 
            IPathfindingService pathfinding, 
            IInputService input)
        {
            _playerPartyProvider = playerPartyProvider;
            _pathfinding = pathfinding;
            _input = input;
        }

        public void Start()
        {
            _input.ActionStarted += _onInputActionStarted;
            _input.ActionEnded += _onInputActionEnded;
            
            _playerPartyProvider.CharacterAdded += _onCharacterAdded;

            foreach (CharacterView characterView in _playerPartyProvider.Heroes)
            {
                _followers.Add(characterView, new CharacterPathfindingFollower(_pathfinding, characterView.AgentSettings));
            }
        }
        
        public void Dispose()
        {
            _input.ActionStarted -= _onInputActionStarted;
            _input.ActionEnded -= _onInputActionEnded;
            
            _playerPartyProvider.CharacterAdded -= _onCharacterAdded;
        }

        private void _onInputActionStarted(EInputAction action)
        {
            if (action != EInputAction.LeftClick)
            {
                return;
            }

            CharacterView character = _playerPartyProvider.CurrentCharacter;
            IPathfindingFollower followers = _followers[character];
            followers.SetPath(character.Body.position, _input.MouseWorldPosition);
            PathUpdated?.Invoke(character, followers.Path, followers.PointIndex);
        }

        private void _onInputActionEnded(EInputAction action)
        {
            if (action != EInputAction.RightClick)
            {
                return;
            }

            foreach (CharacterView character in _playerPartyProvider.Heroes)
            {
                IPathfindingFollower follower = _followers[character];
             
                if (follower.HasActivePath && _followers[character].TryMoveToNextWorldPosition(out Vector3 nextPosition))
                {
                    character.Body.position = nextPosition;
                    character.Model.Position = new Position(nextPosition.x, nextPosition.y);
                }
            }

            CharacterView currentCharacter = _playerPartyProvider.CurrentCharacter;
            IPathfindingFollower currentFollower = _followers[currentCharacter];
            PathUpdated?.Invoke(currentCharacter, currentFollower.Path, currentFollower.PointIndex);
        }


        private void _onCharacterAdded(CharacterView character)
        {
            if (_followers.ContainsKey(character))
            {
                return;
            }
            
            _followers.Add(character, new CharacterPathfindingFollower(_pathfinding, character.AgentSettings));
        }

    }
}
