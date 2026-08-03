using System;
using Code.Game.Characters;
using Code.Game.Characters.Hero;
using Core.Input;
using UnityEngine;
using VContainer.Unity;

namespace Code.Game.World
{
    public class PlayerSelection : IStartable, IDisposable
    {
        private readonly IInputService _input;
        private readonly IPlayerPartyProvider _playerPartyProvider;


        public PlayerSelection(IInputService input, IPlayerPartyProvider playerPartyProvider)
        {
            _input = input;
            _playerPartyProvider = playerPartyProvider;
        }

        public void Start()
        {
            _input.ActionStarted += _onInputActionStarted;
        }

        public void Dispose()
        {
            _input.ActionStarted -= _onInputActionStarted;
        }

        private void _onInputActionStarted(EInputAction action)
        {
            if (action is EInputAction.LeftClick)
            {
                Collider2D collider2D = Physics2D.OverlapPoint(_input.MouseWorldPosition);

                if (collider2D.TryGetComponent(out CharacterView view))
                {
                    _playerPartyProvider.SetCurrentCharacter(view);
                }
            }
        }
    }
}