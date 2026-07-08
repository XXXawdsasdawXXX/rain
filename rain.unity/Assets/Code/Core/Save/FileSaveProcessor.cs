#nullable enable
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Save
{
    public sealed class FileSaveProcessor : ISaveProcessor
    {
        public async UniTask WriteTextAsync(
            string relativePath,
            string content,
            CancellationToken cancellationToken = default)
        {
            string absolutePath = SavePathProvider.GetAbsolutePath(relativePath);
            string? directory = Path.GetDirectoryName(absolutePath);

            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await UniTask.RunOnThreadPool(
                () => File.WriteAllText(absolutePath, content),
                cancellationToken: cancellationToken);
        }

        public async UniTask<string> ReadTextAsync(
            string relativePath,
            CancellationToken cancellationToken = default)
        {
            string absolutePath = SavePathProvider.GetAbsolutePath(relativePath);

            if (!File.Exists(absolutePath))
            {
                return string.Empty;
            }

            return await UniTask.RunOnThreadPool(
                () => File.ReadAllText(absolutePath),
                cancellationToken: cancellationToken);
        }

        public bool Exists(string relativePath)
        {
            return File.Exists(SavePathProvider.GetAbsolutePath(relativePath));
        }

        public void Delete(string relativePath)
        {
            string absolutePath = SavePathProvider.GetAbsolutePath(relativePath);

            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
            }
        }
    }
}