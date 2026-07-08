using System;
using System.Collections.Generic;
using System.Linq;
using Essential;

namespace Code.UI.Components.Window
{
    public abstract class AUIWindowNavigator
    {
        protected readonly Dictionary<Type, AUIWindowView> views = new();

        
        protected AUIWindowNavigator(IEnumerable<AUIWindowView> windows)
        {
            AUIWindowView[] array = windows.ToArray();
            
            foreach (AUIWindowView view in array)
            {
                Type type = view.GetType();

                if (views.ContainsKey(type))
                {
                    Log.Error(this, $"[construct] views collection already has view with type {type.Name}");
                    continue;
                }   
                
                views.Add(type, view);
            }
        }

        public void Open(Type viewType)
        {
            if (views.TryGetValue(viewType, out AUIWindowView view))
            {
                if (view.IsOpened)
                {
                    Log.Error(this, $"[open] unable to open the view with type {viewType.Name} because it is already open");
                    return;
                }
                
                view.Open();
            }
            else
            {
                Log.Error(this, $"[open] can not find view with type {viewType.Name}");
            }
        }

        public void Close(Type viewType)
        {
            if (views.TryGetValue(viewType, out AUIWindowView view))
            {
                if (!view.IsOpened)
                {
                    Log.Error(this, $"[close] unable to close the view with type {viewType.Name} because it is already close");
                    return;
                }
                
                view.Close();
            }
            else
            {
                Log.Error(this, $"[close] can not find view with type {viewType.Name}");
            }
        }

        protected TView getView<TView>() where TView : AUIWindowView
        {
            Type viewType = typeof(TView);
            
            if (views.TryGetValue(viewType, out AUIWindowView view))
            {
                return view as TView;
            }

            Log.Error(this, $"[getView] can not find view with type {viewType.Name}");

            return null;
        }
    }
}