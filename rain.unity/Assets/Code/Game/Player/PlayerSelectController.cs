using System;
using Code.Game.Characters.Hero;
using Core.Input;
using JetBrains.Annotations;
using UnityEngine;
using VContainer.Unity;

namespace Code.Game.Characters
{
    [UsedImplicitly]
    public class PlayerSelectController : IStartable, IDisposable, ITickable
    {
        public bool IsClicking{ get; private set; }
        public Vector3 StartClick { get; private set; }
        public Vector3 EndClick { get; private set; }
        
        private readonly IInputService _input;
        private readonly IPlayerPartyProvider _party;


        public PlayerSelectController(IInputService input, IPlayerPartyProvider party)
        {
            _input = input;
            _party = party;
        }

        public void Start()
        {
            _input.ActionStarted += _inputActionStarted;
            _input.ActionEnded += _onInputActionEnded;
        }

        public void Dispose()
        {
            _input.ActionStarted -= _inputActionStarted;
            _input.ActionEnded -= _onInputActionEnded;
        }

        public void Tick()
        {
            if (IsClicking)
            {
                EndClick = _input.MouseWorldPosition;
            }
        }

        private void _inputActionStarted(EInputAction action)
        {
            if (action == EInputAction.LeftClick)
            {
                StartClick = _input.MouseWorldPosition;
                IsClicking = true;
            }
        }

        private void _onInputActionEnded(EInputAction action)
        {
            if (action == EInputAction.LeftClick)
            {
                EndClick = _input.MouseWorldPosition;
                IsClicking = false;
            }
        }
    }
}