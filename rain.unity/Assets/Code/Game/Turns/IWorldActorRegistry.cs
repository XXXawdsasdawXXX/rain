using System;
using System.Collections.Generic;
using Code.Game.StateMachine;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Code.Game.World.Turns
{
    // IWorldActorRegistry: Register / Unregister / перебор;
    // слушает PhaseChanged, на WorldTurn вызывает всем OnWorldTurnBegin().
    // Контекст минимальный: ссылка на CharacterView + IPathfindingFollower (для героев). Не раздувать.
    public interface IWorldActorRegistry
    {
        void Register(IWorldTurnBeginListener actor);
    }

    [UsedImplicitly]
    public class WorldActorRegistry : IWorldActorRegistry, IStartable, IDisposable
    {
        private readonly IGamePhaseService _phaseService;

        private readonly List<IWorldTurnBeginListener> _beginListener = new();

        private bool _isTriggered;


        public WorldActorRegistry(IGamePhaseService phaseService)
        {
            _phaseService = phaseService;
        }
        
        public void Start()
        {
            _phaseService.PhaseChanged += _onPhaseChanged;
        }

        public void Dispose()
        {
            _phaseService.PhaseChanged -= _onPhaseChanged;
        }

        public void Register(IWorldTurnBeginListener actor)
        {
            _beginListener.Add(actor);
        }

        private void _onPhaseChanged(EGamePhase phase)
        {
            if (phase is EGamePhase.WorldTurn)
            {
                if (_isTriggered)
                {
                   return;
                }
                
                foreach (IWorldActor actor in _beginListener)   
                {
                    actor.OnWorldTurnBegin();
                }

                _isTriggered = true;
            }
            else if(phase is EGamePhase.PlayerTurn && _isTriggered)
            {
                _isTriggered = false;
            }
        }
    }
}