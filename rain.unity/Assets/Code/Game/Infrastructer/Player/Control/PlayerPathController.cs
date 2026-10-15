using System;
using Code.Game.Characters.Hero;
using Code.Game.StateMachine;
using Code.Game.World.Turns.Orders;
using Core.Input;
using JetBrains.Annotations;
using UnityEngine;
using VContainer.Unity;

namespace Code.Game.Characters
{
    [UsedImplicitly]
    public class PlayerPathController : IStartable, IDisposable
    {
        private readonly IPlayerPartyProvider _playerPartyProvider;
        private readonly IGamePhaseService _gamePhaseService;
        private readonly IInputService _input;

        
        public PlayerPathController(
            IPlayerPartyProvider playerPartyProvider, 
            IInputService input, 
            IGamePhaseService gamePhaseService)
        {
            _playerPartyProvider = playerPartyProvider;
            _input = input;
            _gamePhaseService = gamePhaseService;
        }

        public void Start()
        {
            _input.ActionEnded += _onInputActionEnded;
        }

        public void Dispose()
        {
            _input.ActionEnded -= _onInputActionEnded;
        }

        private void _onInputActionEnded(EInputAction action)
        {
            if (_gamePhaseService.Phase is EGamePhase.PlayerTurn)
            {
                if (action == EInputAction.LeftClick)
                {
                    if (Time.unscaledTime - _input.StartLeftClickTime < IInputService.HOLDDELEY_DURATION)
                    {
                        _setPathForCurrentHero();
                    }
                }
            }
        }
        
        private void _setPathForCurrentHero()
        {
            CharacterWorldActor hero = _playerPartyProvider.CurrentCharacter; 
            hero.SetOrder(new MoveToPointOrder(hero, _input.MouseWorldPosition)); 
        }
    }
}
