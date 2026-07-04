using Cysharp.Threading.Tasks;

namespace Essential.Common.Interfaces
{
    public interface IInitializable
    {
        bool IsInitialized { get; set; }
        UniTask Initialize();
        
    }
}