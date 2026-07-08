using System;
using VContainer.Unity;

namespace Code.UI.Components.Window
{
    public abstract class AUIWindowController<TView> : IStartable, IDisposable where TView : AUIWindowView
    {
        protected TView view;
        
        protected AUIWindowController(TView view)
        {
            this.view = view;
        }

        public virtual void Start()
        {
            subscribe();
        }

        public void Dispose()
        {
            unSubscribe();
        }

        protected abstract void subscribe();

        protected abstract void unSubscribe();
    }
}