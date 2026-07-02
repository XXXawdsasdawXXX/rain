using Core.GameLoop;
using Core.ServiceLocator.MarkerInterfaces;
using Cysharp.Threading.Tasks;

namespace Core.Save
{
    public abstract class Saver<T> : IService, IInitializeListener, IExitListener where T : SavedModel
    {
        public bool IsInitialized { get; set; }
        public T Current { get; protected set; }


        public UniTask Initialize()
        {
            if (!SaveService.Load(out T model))
            {
                model = getNewModel();
            }
    
            Current = model;
            
            return UniTask.CompletedTask;
        }

        public void GameExit()
        {
            Save();
        }

        public void Save()
        {
            SaveService.Save(Current);
        }

        protected abstract T getNewModel();
    }
}