using System;
using Code.Game.Characters.Hero;
using Code.Game.StateMachine;
using Code.Game.World.Characters;
using Code.Game.World.Turns.Orders;
using Code.Pathfinding;
using Core.Input;
using JetBrains.Annotations;
using UnityEngine;
using VContainer.Unity;

namespace Code.Game.Characters
{
    public class PlayerPathController : IStartable, IDisposable
    {
        private readonly IPlayerPartyProvider _playerPartyProvider;
        private readonly IGamePhaseService _gamePhaseService;
        private readonly IPlayerInputService _input;
        private readonly ITilemapView _tilemap;


        public PlayerPathController(
            IPlayerPartyProvider playerPartyProvider, 
            IPlayerInputService input, 
            IGamePhaseService gamePhaseService, 
            ITilemapView tilemap)
        {
            _playerPartyProvider = playerPartyProvider;
            _input = input;
            _gamePhaseService = gamePhaseService;
            _tilemap = tilemap;
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
