using System;
using Code.Game.Infrastructure.StateMachine;
using Code.Game.Infrastructure.Turns.Orders;
using Code.Game.Pathfinding.View;
using Code.Game.World.Characters;
using Core.Input;
using UnityEngine;
using VContainer.Unity;

namespace Code.Game.Player.Control
{
    public class PlayerPathController : IStartable, IDisposable
    {
        private readonly IPlayerPartyProvider _playerPartyProvider;
        private readonly IGamePhaseService _gamePhaseService;
        private readonly IPlayerInputService _input;
        private readonly ITilemapView _tilemap;
        private readonly IAbilityTargeting _abilityTargeting;


        public PlayerPathController(
            IPlayerPartyProvider playerPartyProvider, 
            IPlayerInputService input, 
            IGamePhaseService gamePhaseService, 
            ITilemapView tilemap,
            IAbilityTargeting abilityTargeting)
        {
            _playerPartyProvider = playerPartyProvider;
            _input = input;
            _gamePhaseService = gamePhaseService;
            _tilemap = tilemap;
            _abilityTargeting = abilityTargeting;
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
            if (_abilityTargeting.IsTargeting)
            {
                return;
            }
            
            if (_gamePhaseService.Phase is EGamePhase.PlayerTurn)
            {
                if (action == EInputAction.LeftClick)
                {
                    if (Time.unscaledTime - _input.StartLeftClickTime < IPlayerInputService.HOLDDELEY_DURATION)
                    {
                        _setPathForCurrentHero();
                    }
                }
            }
        }
        
        private void _setPathForCurrentHero()
        {
            CharacterActor hero = _playerPartyProvider.CurrentCharacter;
            Vector3Int cellPosition = _tilemap.WorldAsCell(_input.MouseWorldPosition);
            hero.SetOrder(new MoveToPointOrder(hero, cellPosition)); 
        }
    }
}
