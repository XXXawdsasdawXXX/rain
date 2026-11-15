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
    public interface IAbilityTargeting
    {
        bool IsTargeting { get; }
        void BeginTargeting(AbilityModel ability);

    }
    public class AbilityTargetingController: IAbilityTargeting, IStartable, IDisposable
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
            _input.ActionEnded += _onInputActionEnded;
        }

        public void Dispose()
        {
            _input.ActionEnded -= _onInputActionEnded;
        }
        
        public void BeginTargeting(AbilityModel ability)
        {
            _cancel();
            
            _ability = ability;
            
            _input.MouseCellPosition.SubscribeProperty(_onMouseCellChanged);
            
            _refreshPreview();
        }

        private void _cancel()
        {
            if (_ability == null)
            {
                return;
            }
            
            _input.MouseCellPosition.UnsubscribeProperty(_onMouseCellChanged);
            _ability = null;
            _tilemap.Clear();
            _tilemap.DrawLastPath();
        }

        private void _confirm()
        {
            _cancel();
        }
        
        private void _onInputActionEnded(EInputAction action)
        {
            if (_gamePhaseService.Phase is not EGamePhase.PlayerTurn)
            {
                return;
            }

            switch (action)
            {
                case EInputAction.None:
                case EInputAction.Interaction:
                case EInputAction.DoubleClick:
                case EInputAction.Scroll:
                case EInputAction.LeftClick:
                default:
                    break;
                
                case EInputAction.HoldLeftClick:
                    _confirm();
                    break;
                
                case EInputAction.Esc:
                        _cancel();
                        break;
                
                case EInputAction.RightClick:
                    if (IsTargeting)
                    {
                        _cancel();
                    }
                    else
                    {
                        _gamePhaseService.RequestEndPlayerTurn();
                    }
                    break;
            }
        }
        
        private void _onMouseCellChanged(Vector3Int _)
        {
            _refreshPreview();
        }

        private void _refreshPreview()
        {
            _tilemap.Clear();

            if (_ability == null)
            {
                return;
            }
            
            bool atCursor = _ability.TargetMode is ETargetingMode.AtCursor;
            
            CharacterActor hero = _playerPartyProvider.CurrentCharacter;

            Vector3Int origin = atCursor ? _input.MouseCellPosition.Value : hero.Model.Position;

            IReadOnlyList<Vector3Int> cells = AbilityResolver.GetCells(_ability.Zone, origin);

            _tilemap.DrawZone(cells);
        }
    }
}
