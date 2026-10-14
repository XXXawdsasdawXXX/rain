using System;
using Cysharp.Threading.Tasks;

namespace Code.Game.StateMachine
{
    public abstract class AState
    {
        private event Action<Type> _requestedTransition;

        
        protected AState(Action<Type> requestTransition)
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