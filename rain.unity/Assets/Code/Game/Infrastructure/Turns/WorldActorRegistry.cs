using System;
using System.Collections.Generic;
using Code.Game.Infrastructure.StateMachine;
using Code.Game.World.Turns;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Code.Game.Infrastructure.Turns
{
    [UsedImplicitly]
    public class WorldActorRegistry : IRegister<IWorldTurnBeginListener>, IStartable, IDisposable
    {
        private readonly IGamePhaseService _phaseService;

        private readonly List<IWorldTurnBeginListener>[] _beginListener =
        {
            new(), //characters
            new(), //obstacles
            new()  //other
        };

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
            List<IWorldTurnBeginListener> bucket = _beginListener[(int)actor.Priority];
            
            if (!bucket.Contains(actor))
            {
                bucket.Add(actor);
            }
        }

        public void UnRegister(IWorldTurnBeginListener actor)
        {
            List<IWorldTurnBeginListener> bucket = _beginListener[(int)actor.Priority];
            
            if (bucket.Contains(actor))
            {
                bucket.Remove(actor);
            }
        }

        private void _onPhaseChanged(EGamePhase phase)
        {
            if (phase is EGamePhase.WorldTurn)
            {
                if (_isTriggered)
                {
                   return;
                }

                foreach (List<IWorldTurnBeginListener> bucket in _beginListener)
                {
                    foreach (IWorldTurnBeginListener beginListener in bucket)
                    { 
                        beginListener.OnWorldTurnBegin();
                    }
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