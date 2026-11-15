using System;
using System.Collections.Generic;
using System.Linq;
using Code.Game.Ability;
using Code.Game.Ability.Effect;
using Code.Game.Infrastructure.StateMachine;
using Code.Game.Pathfinding.View;
using Code.Game.World;
using Code.Game.World.Characters;
using Core.Input;
using UnityEngine;
using VContainer.Unity;

namespace Code.Game.Player.Control
{
    public interface IAbilityTargeting
    {
        bool IsTargeting { get; }
        void BeginTargeting(AbilityModel ability);

    }
    
    public struct AbilityOutcome
    {
        public CharacterActor Owner;
        public IReadOnlyList<IHealth> Targets;
        public AbilityModel Ability;

        public override string ToString()
        {
            return $"{Owner.Model.Name} use {Ability.Name}. targets count = {Targets.Count}";
        }
    }
    public class AbilityTargetingController: IAbilityTargeting, IStartable, IDisposable
    {
   
        
        private readonly IPlayerPartyProvider _playerPartyProvider;
        private readonly IGamePhaseService _gamePhaseService;
        private readonly IPlayerInputService _input;
        private readonly ITileDrawer _tilemap;
        private readonly IEntityRegistry _entityRegistry;

        private AbilityModel _ability;
        
        public bool IsTargeting => _ability != null;


        public AbilityTargetingController(
            IPlayerPartyProvider playerPartyProvider, 
            IPlayerInputService input, 
            IGamePhaseService gamePhaseService, 
            ITileDrawer tilemap,
            IEntityRegistry entityRegistry)
        {
            _playerPartyProvider = playerPartyProvider;
            _input = input;
            _gamePhaseService = gamePhaseService;
            _tilemap = tilemap;
            _entityRegistry = entityRegistry;
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
            if (_ability == null)
            {
                return;
            }

            IReadOnlyList<Vector3Int> zone = AbilityResolver.GetCells(
                _ability.Zone,
                _getAbilityOriginCell(_ability, _playerPartyProvider.CurrentCharacter));

            AbilityOutcome outcome = new()
            {
                Owner = _playerPartyProvider.CurrentCharacter,
                Targets = _findTargets(zone),
                Ability = _ability
            };


            IAbilityEffect effect = _ability.Type switch
            {
                EAbilityType.Damage => new DamageAbilityEffect(),
                _ => null
            };

            effect?.Apply(outcome);

            Debug.Log(outcome.ToString());

            _cancel();
        }

        private  List<IHealth> _findTargets(IReadOnlyList<Vector3Int> zone)
        {
            HashSet<IHealth> targets = new HashSet<IHealth>();
            foreach (Vector3Int cell in zone)
            {
                if (_entityRegistry.TryGetEntities(cell, out List<IEntityView> entities))
                {
                    foreach (IEntityView entity in entities)
                    {
                        if (entity.TryGetHealth(out IHealth health))
                        {
                            targets.Add(health);
                        }
                    }
                }
            }
            
            return targets.ToList();
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

            Vector3Int origin = _getAbilityOriginCell(_ability, hero);

            IReadOnlyList<Vector3Int> cells = AbilityResolver.GetCells(_ability.Zone, origin);

            _tilemap.DrawZone(cells);
        }

        private Vector3Int _getAbilityOriginCell(AbilityModel ability, CharacterActor actor)
        {
            return ability.TargetMode is ETargetingMode.AtCursor 
                ? _input.MouseCellPosition.Value 
                : actor.Model.Position;
        }
    }
}
