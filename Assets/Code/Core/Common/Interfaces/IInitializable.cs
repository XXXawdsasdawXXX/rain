using Cysharp.Threading.Tasks;

namespace Core.Common.Interfaces
{
    public interface IInitializable
    {
        bool IsInitialized { get; set; }
        UniTask Initialize();
        
    }
}