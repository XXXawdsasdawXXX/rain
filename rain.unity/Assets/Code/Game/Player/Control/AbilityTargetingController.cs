using System;
using System.Collections.Generic;
using System.Linq;
using Code.Game.Characters.Hero;
using Code.Game.StateMachine;
using Code.Game.World;
using Code.Game.World.Abilities;
using Code.Game.World.Characters;
using Code.Pathfinding;
using Core.Input;
using Essential;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Code.Game.Characters
{
    public interface IAbilityTargeting
    {
        bool IsTargeting { get; }
        void BeginTargeting(AbilityModel ability);

    }
    public class AbilityTargetingController: IAbilityTargeting, IStartable, IDisposable
    {
        public struct ActionOutcome
        {
            public CharacterActor Owner;
            public IReadOnlyList<IHealth> Targets;
            public AbilityModel Ability;

            public override string ToString()
            {
                return $"{Owner.Model.Name} use {Ability.Name}. targets count = {Targets.Count}";
            }
        }
        
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
            
            ActionOutcome outcome = new()
            {
                Owner = _playerPartyProvider.CurrentCharacter,
                Targets = _findTargets(zone),
                Ability = _ability
            };

            switch (_ability.Type)
            {
                case EAbilityType.None:
                case EAbilityType.Heal:
                case EAbilityType.Harvest:
                case EAbilityType.ApplyEffect:
                default:
                    break;
                case EAbilityType.Damage:
                    //var damage = _ability.Stats

                    float damage = 0;
                    foreach (Stat stat in _ability.Stats)
                    {
                        switch (stat.Type)
                        {
                            case EStatType.None:
                            case EStatType.Health:
                            case EStatType.Noise:
                            case EStatType.Energy:
                            default:
                                break;
                            case EStatType.Damage:
                                damage += stat.Value;
                                break;
                            
                            case EStatType.CriticalChance:
                                if (Random.Range(0, 100) < stat.Value)
                                {
                                    damage *= 1.2f;
                                }

                                break;
                        }
                    }

                    if (damage < 0)
                    {
                        return;
                    }
                    foreach (IHealth target in outcome.Targets)
                    {
                        target.TakeDamage(damage);
                    }
                    break;
            }
        
            
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
