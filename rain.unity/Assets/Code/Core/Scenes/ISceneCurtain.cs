using Cysharp.Threading.Tasks;

namespace Core.Scenes
{
    public interface ISceneCurtain
    {
        UniTask Show();
        UniTask Hide();
    }
}