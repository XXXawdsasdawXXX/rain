using Core.GameLoop;
using Core.Save.Models;
using Core.ServiceLocator.MarkerInterfaces;
using Cysharp.Threading.Tasks;
using Essential;

namespace Core.Save
{
    public abstract class Saver<T> : IService, IInitializeListener, IExitListener where T : SavedModel
    {
        public bool IsInitialized { get; set; }
        public T Current { get; protected set; }


        public async UniTask Initialize()
        {
            T save = await SaveService.Load<T>();
            
            if (save == null)
            { 
                Log.Info(this, $"new load data");
                save = getNewModel();
            }
    
            Current = save;

            Save();
            
            Log.Info(this, $"Load data {Current}");
        }

        public void GameExit()
        {
            Save();
        }

        public void Save()
        {
            SaveService.Save(Current);
            
            Log.Info(this, $"Save data {Current}");
        }

        protected abstract T getNewModel();
    }
}