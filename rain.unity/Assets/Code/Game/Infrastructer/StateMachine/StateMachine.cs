using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Essential;

namespace Code.Game.StateMachine
{
    public abstract class StateMachine<TState>   where TState : AState
    {
        protected Dictionary<Type, TState> states;
        
        protected TState currentState;
        
        
        protected async UniTask setState(Type type)
        {
            try
            {
                if (currentState != null)
                {
                    Log.Info(this, $"set state - from {currentState.GetType().Name} to {type.Name}", Log.Orange);
                    
                    await currentState.Exit();
                }
                
                currentState = states[type];

                Log.Info(this, $"set state - enter to {type.Name}", Log.Orange);

                await states[type].Enter();
            }
            catch (Exception e)
            {
                Log.Error(this, e.Message);
            }
        }
    }
}