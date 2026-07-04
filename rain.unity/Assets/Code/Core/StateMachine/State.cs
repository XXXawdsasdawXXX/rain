using System;
using Cysharp.Threading.Tasks;

namespace Core.StateMachine
{
    public abstract class State
    {
        private event Action<Type> _requestedTransition;

        
        protected State(Action<Type> requestTransition)
        {
            _requestedTransition = requestTransition;
        }

        public abstract UniTask Enter();

        public abstract UniTask Exit();
        

        protected void requestTransition(Type nextState)
        {
            _requestedTransition?.Invoke(nextState);
        }
    }
}