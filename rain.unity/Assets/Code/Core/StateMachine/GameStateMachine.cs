using System;
using System.Collections.Generic;
using Core.Scenes;
using Core.StateMachine.States;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Core.StateMachine
{
    public sealed class GameStateMachine : StateMachine<State>, IStartable
    {
        private readonly ISceneCurtain _sceneCurtain;
        
        private State _currentState;


        public GameStateMachine(ISceneCurtain sceneCurtain)
        {
            _sceneCurtain = sceneCurtain;
            
            states = new Dictionary<Type, State>()
            {
                {typeof(MenuState), new MenuState(_onRequestTransition, _sceneCurtain)}
            };
        }

        public void Start()
        {
            setState(typeof(MenuState)).Forget();
        }

        private async void _onRequestTransition(Type nextState)
        {
            await _currentState.Exit();

            _currentState = states[nextState];

            await _currentState.Enter();
        }
    }
}