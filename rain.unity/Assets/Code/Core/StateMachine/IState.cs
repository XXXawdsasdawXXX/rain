using Cysharp.Threading.Tasks;
using Essential.Common.Interfaces;

namespace Core.StateMachine
{
    public interface IState : IInitializable
    {
        UniTask Enter();

        UniTask Exit();
    }
}