using System;
using Core.Input;
using JetBrains.Annotations;
using UnityEngine;
using VContainer.Unity;

namespace Code.Game.StateMachine
{
    public enum EGamePhase
    {
        Paused, 
        WorldTurn,
        PlayerTurn
    }
    
    public interface IGamePhaseService 
    {
        EGamePhase Phase { get; }
        event Action<EGamePhase> PhaseChanged;
        void TogglePause();          // кнопка / Esc
        void RequestEndPlayerTurn(); // ПКМ / End Turn
    }
    
    
    [UsedImplicitly]
    public class GamePhaseService : IGamePhaseService, IStartable, IDisposable, ITickable
    {
        private const float WORLD_PHASE_DURATION = 0.5F;
        
        public event Action<EGamePhase> PhaseChanged;
        public EGamePhase Phase { get; private set; }
        
        private readonly IInputService _input;
        
        private EGamePhase _previousPhase;
        private float _worldPhaseElapsed;
        

        public GamePhaseService(IInputService input)
        {
            _input = input;
        }
        
        public void Start()
        {
            Phase = EGamePhase.PlayerTurn;
            _input.ActionStarted += _onInputActionEnded;
        }

        public void Dispose()
        {
            _input.ActionStarted -= _onInputActionEnded;
            
            PhaseChanged = null;
        }

        public void Tick()
        {
            if (Phase is EGamePhase.WorldTurn)
            {
                _worldPhaseElapsed += Time.deltaTime;
                if (_worldPhaseElapsed > WORLD_PHASE_DURATION)
                {
                    Phase = EGamePhase.PlayerTurn;
                    PhaseChanged?.Invoke(Phase);
                }
            }
        }

        public void TogglePause()
        {
            if (Phase is EGamePhase.Paused)
            {
                Phase = _previousPhase is EGamePhase.PlayerTurn ? EGamePhase.PlayerTurn : EGamePhase.WorldTurn;
            }
            else
            {
                _previousPhase = Phase;
                Phase = EGamePhase.Paused;
            }

            PhaseChanged?.Invoke(Phase);
        }

        public void RequestEndPlayerTurn()
        {
            if (Phase is EGamePhase.PlayerTurn)
            {
                _worldPhaseElapsed = 0;
                Phase = EGamePhase.WorldTurn;
                PhaseChanged?.Invoke(Phase);
            }
        }

        private void _onInputActionEnded(EInputAction action)
        {
            if (action is EInputAction.RightClick)
            {
                RequestEndPlayerTurn();
            }
        }
    }
}