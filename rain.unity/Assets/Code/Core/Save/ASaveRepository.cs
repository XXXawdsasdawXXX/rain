using System.Threading;
using Cysharp.Threading.Tasks;
using Essential;
using Essential.Extensions;

namespace Core.Save
{
    public abstract class ASaveRepository<T>
    {
        public T Data { get; private set; }
        public bool WasChanged { get; protected set; }
        
        private readonly ISaveProcessor _processor;

        
        protected ASaveRepository(ISaveProcessor processor)
        {
            _processor = processor;
        }

        public async UniTask<T> LoadOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            string json = await _processor.ReadTextAsync(SavePathProvider.SettingsFile, cancellationToken);

            if (string.IsNullOrWhiteSpace(json))
            {
                Data = createDefault();
            }
            else
            {
                Data = json.AsData<T>() ?? createDefault();
            }
            
            return Data;
        }

        public async UniTask SaveAsync(CancellationToken cancellationToken = default)
        {
            string json = Data.AsJson();
            await _processor.WriteTextAsync(SavePathProvider.SettingsFile, json, cancellationToken);
            
            Log.Info(this, $"[SaveAsync] saved:\n{json}");
        }

        protected abstract T createDefault();
    }
}