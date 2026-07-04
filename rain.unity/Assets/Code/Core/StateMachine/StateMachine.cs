using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Essential;

namespace Core.StateMachine
{
    public abstract class StateMachine<TState>   where TState : State
    {
        protected Dictionary<Type, TState> states;
        
        protected TState currentState;
        
        
        public async UniTask SwitchState(Type type)
        {
            try
            {
                if (currentState != null)
                {
                    await currentState.Exit();
                    Log.Info(this, $"switch state - exit from {currentState.GetType().Name}");
                }

                Log.Info(this, $"switch state - set state to {type.Name}");
              
                await setState(type);
            }
            catch (Exception e)
            {
                Log.Error(this, e.Message);
            }
        }

        protected virtual async UniTask setState(Type type)
        {
            try
            {
                Log.Info(this, $"set state  - from {currentState?.GetType().Name} to {type.Name} ");
                currentState = states[type];

       
            
                Log.Info(this, $"set state  - enter {type.Name} ");
                await states[type].Enter();
            }
            catch (Exception e)
            {
                Log.Error(this, e.Message);
            }
        }
    }
}