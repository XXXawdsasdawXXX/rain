using System;
using System.Collections.Generic;
using System.Linq;
using Code.Game.StateMachine;
using Cysharp.Threading.Tasks;
using Essential;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Code.Game.World.Turns
{
    [UsedImplicitly]
    public class WorldActorRegistry : IRegister<IWorldTurnBeginListener>, IStartable, IDisposable
    {
        private readonly IGamePhaseService _phaseService;

        private List<IWorldTurnBeginListener>[] _beginListener =
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