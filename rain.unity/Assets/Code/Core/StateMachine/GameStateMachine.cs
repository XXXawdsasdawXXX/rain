using System;
using System.Collections.Generic;
using Core.AssetManagement;
using Core.StateMachine.States;
using Cysharp.Threading.Tasks;

namespace Core.StateMachine
{
    public sealed class GameStateMachine : StateMachine<IState>
    {
        private IState _currentState;

        public GameStateMachine()
        {
         
        }

        public GameStateMachine(AssetManagement.Installers installers, Configurations configurations)
        {
            states = new Dictionary<Type, IState>()
            {
                { typeof(BootstrapState), new BootstrapState(this, installers, configurations)},
                { typeof(MainMenuState), new MainMenuState() },
                { typeof(CoreGameState) , new CoreGameState()}
            };
            
            setState(typeof(BootstrapState)).Forget();
        }
    }
}