using System;
using System.Collections.Generic;
using Core.GameLoop;
using Core.Save;
using Core.Save.Models;
using Core.ServiceLocator;
using Core.ServiceLocator.MarkerInterfaces;
using Cysharp.Threading.Tasks;

namespace Code.UI.Windows.Base
{
    public class UIWindowManager : Essential.Mono, IService, 
        IInitializeListener, IStartListener, ISubscriber
    {
        public bool IsInitialized { get; set; }

        private IWindowController[] _windows; 
        
        private readonly Dictionary<Type, IWindowController> _windowControllers = new();
        
        private IWindowController _lastOpenedWindow;
        private GamesSaver _gameSaver;


        public async UniTask Initialize()
        {
            _windows = GetComponentsInChildren<IWindowController>(true);
            _gameSaver = Container.Instance.GetService<GamesSaver>();
            
            foreach (IWindowController windowController in _windows)
            {
                if (windowController.IsDetached)
                {
                    continue;
                }
                
                Type type = windowController.GetType();
                
                _windowControllers.Add(type, windowController);
            }
            
            foreach (IWindowController windowController in _windows)
            {
                await windowController.InitializeWindow(this);
            }
        }

        public UniTask GameStart()
        {
            foreach ((Type _, IWindowController windowController) in _windowControllers)
            {
                windowController.LoadWindow(_gameSaver.Current);
            }

            foreach ((Type _, IWindowController windowController) in _windowControllers)
            {
                windowController.StartWindow();
            }
            
            return UniTask.CompletedTask;
        }

        public void Subscribe()
        {
            foreach ((Type type, IWindowController windowController) in _windowControllers)
            {
                windowController.SubscribeToEvents(true);
            }
        }

        public void Unsubscribe()
        {
            foreach ((Type _, IWindowController windowController) in _windowControllers)
            {
                windowController.SubscribeToEvents(false);
            }
        }

        public void OpenWindow<T>() where T : class, IWindowController
        {
            Type type = typeof(T);
            
            _checkCollection<T>(type);

            _windowControllers[type].Open();
        }
        
        public void CloseWindow<T>() where T : class, IWindowController
        {
            Type type = typeof(T);
            
            _checkCollection<T>(type);

            _windowControllers[type].Close();
        }
        
        public void SwitchWindow<T>() where T : class, IWindowController
        {
            Type type = typeof(T);
            
            _checkCollection<T>(type);
            
            _lastOpenedWindow?.Close();
                    
            _lastOpenedWindow = _windowControllers[type];
                    
            _lastOpenedWindow.Open();
        }

        private void _checkCollection<T>(Type type) where T : class, IWindowController
        {
            if (!_windowControllers.ContainsKey(type))
            {
                foreach (IWindowController window in _windows)
                {
                    if (window is T)
                    {
                        _windowControllers.Add(type, window);
                    }
                }
            }
        }

        public T GetWindow<T>() where T : class, IWindowController
        {
            Type type = typeof(T);
            
            if (_windowControllers.ContainsKey(type))
            {
                return _windowControllers[type] as T;
            }

            foreach (IWindowController window in _windows)
            {
                if (window is T windowController)
                {
                    _windowControllers.Add(type, window);
                        
                    return windowController;
                }
            }

            return null;
        }
    }
}