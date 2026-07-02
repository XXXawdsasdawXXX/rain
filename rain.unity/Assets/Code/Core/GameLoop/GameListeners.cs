using Core.Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace Core.GameLoop
{
    public interface IGameListener
    {
    }

    public interface ISubscriber : IGameListener
    {
        void Subscribe();
        void Unsubscribe();
    }
    
    public interface IInitializeListener : IGameListener
    {
        bool IsInitialized { get; set; }
        UniTask Initialize();
    }

    public interface IStartListener : IGameListener
    {
        UniTask GameStart();
    }
    
    public interface IUpdateListener : IGameListener
    {
        void GameUpdate(float deltaTime);
    }
    
    public interface IFixedUpdateListener : IGameListener
    {
        void GameFixedUpdate(float fixedDeltaTime);
    }
    
    public interface IExitListener : IGameListener
    {
        void GameExit();
    }
}