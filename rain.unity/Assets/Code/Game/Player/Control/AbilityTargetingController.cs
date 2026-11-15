using System;
using System.Collections.Generic;
using Code.Game.Characters.Hero;
using Code.Game.StateMachine;
using Code.Game.World.Abilities;
using Code.Game.World.Characters;
using Code.Pathfinding;
using Core.Input;
using Essential;
using UnityEngine;
using VContainer.Unity;

namespace Code.Game.Characters
{
    public class AbilityTargetingController: IStartable, IDisposable
    {
        private readonly IPlayerPartyProvider _playerPartyProvider;
        private readonly IGamePhaseService _gamePhaseService;
        private readonly IPlayerInputService _input;
        private readonly ITileDrawer _tilemap;

        private AbilityModel _ability;
        
        public bool IsTargeting => _ability != null;


        public AbilityTargetingController(
            IPlayerPartyProvider playerPartyProvider, 
            IPlayerInputService input, 
            IGamePhaseService gamePhaseService, 
            ITileDrawer tilemap)
        {
            _playerPartyProvider = playerPartyProvider;
            _input = input;
            _gamePhaseService = gamePhaseService;
            _tilemap = tilemap;
        }

        public void Start()
        { 
            Log.Info(this, "start");
            _input.ActionEnded += _onInputActionEnded;
        }

        public void Dispose()
        {
            _input.ActionEnded -= _onInputActionEnded;
        }

        /// <summary>
        /// for button
        /// </summary>
        /// <param name="ability"></param>
        public void BeginTargeting(AbilityModel ability)
        {
            Cancel();
            
            _ability = ability;
            
            _input.MouseCellPosition.SubscribeProperty(_onMouseCellChanged);
            
            _refreshPreview();
        }

        public void Cancel()
        {
            if (_ability == null)
            {
                return;
            }
            
            _input.MouseCellPosition.UnsubscribeProperty(_onMouseCellChanged);
            _tilemap.Clear();
            _ability = null;
        }
        
        
        private void _onInputActionEnded(EInputAction action)
        {
            /*if (_gamePhaseService.Phase is EGamePhase.PlayerTurn)
            {
                if (action == EInputAction.LeftClick)
                {
                    if (Time.unscaledTime - _input.StartLeftClickTime < IPlayerInputService.HOLDDELEY_DURATION)
                    {
                        _setPathForCurrentHero();
                    }
                }
            }*/
        }
        
        private void _onMouseCellChanged(Vector3Int mouseCell)
        {
            _refreshPreview();
        }

        private void _refreshPreview()
        {
            _tilemap.Clear();
            
            bool atCursor = _ability.TargetMode is ETargetingMode.AtCursor;
            
            CharacterActor hero = _playerPartyProvider.CurrentCharacter;

            Vector3Int origin = atCursor ? _input.MouseCellPosition.Value : hero.Model.Position;

            IReadOnlyList<Vector3Int> cells = AbilityResolver.GetCells(_ability.Zone, origin);

            _tilemap.DrawZone(cells);
        }
    }
}
