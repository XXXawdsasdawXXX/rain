using System;
using System.Collections.Generic;
using Code.Game.Save;
using Code.Game.StateMachine.States;
using Core.Scenes;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Code.Game.StateMachine
{
    [UsedImplicitly]
    public sealed class GameStateMachine : StateMachine<AState>, IStartable, IDisposable
    {
        public GameStateMachine(ISceneCurtain sceneCurtain, 
            SettingsSaveRepository settingsSave, GameSaveRepository gameSave)
        {
            states = new Dictionary<Type, AState>()
            {
                {typeof(BootstrapState), new BootstrapState(RequestTransition, sceneCurtain, settingsSave, gameSave)},
                {typeof(MenuState), new MenuState(RequestTransition, sceneCurtain, gameSave)},
                {typeof(GameState), new GameState(RequestTransition, sceneCurtain)}
            };
        }

        public void Start()
        {
            setState(typeof(BootstrapState)).Forget();
        }

        public async void RequestTransition(Type nextState)
        {
            await currentState.Exit();
            
            await setState(nextState);
        }

        public void Dispose()
        {
            currentState.Exit().Forget();
        }
    }
}