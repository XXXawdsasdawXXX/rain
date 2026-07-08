using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Save
{
    public interface ISaveProcessor
    {
        UniTask WriteTextAsync(
            string relativePath,
            string content,
            CancellationToken cancellationToken = default);

        UniTask<string> ReadTextAsync(
            string relativePath,
            CancellationToken cancellationToken = default);

        bool Exists(string relativePath);

        void Delete(string relativePath);
    }
}