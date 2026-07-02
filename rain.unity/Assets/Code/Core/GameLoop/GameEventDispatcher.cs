using System.Collections.Generic;
using Core.ServiceLocator.MarkerInterfaces;
using Cysharp.Threading.Tasks;
using Unity.Profiling;

namespace Core.GameLoop
{
    public sealed class GameEventDispatcher : Essential.Mono, IService
    {
        private readonly HashSet<IGameListener> _listeners = new();
        private readonly HashSet<IInitializeListener> _initListeners = new();
        private readonly HashSet<IStartListener> _startListeners = new();
        private readonly HashSet<IUpdateListener> _updateListeners = new();
        private readonly HashSet<IFixedUpdateListener> _fixedUpdateListeners = new();
        private readonly HashSet<IExitListener> _exitListeners = new();
        private readonly HashSet<ISubscriber> _subscribers = new();

        private bool _isStarted;
        

        private void Update()
        {
            if (_isStarted)
            {
                _notifyGameUpdate(UnityEngine.Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (_isStarted)
            {
                _notifyGameFixedUpdate(UnityEngine.Time.fixedDeltaTime);
            }
        }

        private void OnApplicationQuit()
        {
            _notifyGameExit();
        }

        public async UniTask Register(List<IGameListener> listeners)
        {
            _addListeners(listeners);

            await _notifyGameInitialize();
            await _notifySubscribe();
            await _notifyGameStart();

            _isStarted = true;
        }

        public void Dispose()
        {
            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.Unsubscribe();
            }

            _listeners.Clear();
            _initListeners.Clear();
            _startListeners.Clear();
            _updateListeners.Clear();
            _fixedUpdateListeners.Clear();
            _exitListeners.Clear();
            _subscribers.Clear();

            _isStarted = false;
        }

        public async void InitializeListeners(IGameListener[] listeners)
        {
            foreach (IGameListener listener in listeners)
            {
                await _initializeListener(listener);
            }
        }

        private async UniTask _initializeListener(IGameListener listener)
        {
            if (!_listeners.Add(listener))
            {
                return;
            }

            ProfilerMarker marker = new($"AddSpawnableListener: {listener.GetType().Name}");
            marker.Begin();

            if (listener is IInitializeListener initListener && !initListener.IsInitialized)
            {
                await initListener.Initialize();

                initListener.IsInitialized = true;
            }

            if (listener is ISubscriber subscriber)
            {
                subscriber.Subscribe();

                _subscribers.Add(subscriber);
            }

            if (listener is IStartListener startListener)
            {
                await startListener.GameStart();
            }

            if (listener is IUpdateListener updateListener) _updateListeners.Add(updateListener);

            if (listener is IFixedUpdateListener fixedUpdateListener) _fixedUpdateListeners.Add(fixedUpdateListener);

            if (listener is IExitListener exitListener) _exitListeners.Add(exitListener);

            marker.End();
        }

        public void RemoveListeners(IGameListener[] listeners)
        {
            foreach (IGameListener listener in listeners)
            {
                _removeListener(listener);
            }
        }

        private void _removeListener(IGameListener listener)
        {
            if (!_listeners.Remove(listener))
            {
                return;
            }

            if (listener is IUpdateListener updateListener)
            {
                _updateListeners.Remove(updateListener);
            }

            if (listener is IFixedUpdateListener fixedUpdateListener)
            {
                _fixedUpdateListeners.Remove(fixedUpdateListener);
            }

            if (listener is ISubscriber subscriber)
            {
                subscriber.Unsubscribe();

                _subscribers.Remove(subscriber);
            }

            if (listener is IExitListener exitListener)
            {
                _exitListeners.Remove(exitListener);
            }
        }

        private void _addListeners(List<IGameListener> gameListeners)
        {
            foreach (IGameListener listener in gameListeners)
            {
                if (!_listeners.Add(listener)) continue;

                if (listener is IInitializeListener initListener) _initListeners.Add(initListener);

                if (listener is ISubscriber subscriber) _subscribers.Add(subscriber);

                if (listener is IStartListener startListener) _startListeners.Add(startListener);

                if (listener is IUpdateListener updateListener) _updateListeners.Add(updateListener);

                if (listener is IFixedUpdateListener fixedUpdateListener)
                    _fixedUpdateListeners.Add(fixedUpdateListener);

                if (listener is IExitListener exitListener) _exitListeners.Add(exitListener);
            }
        }

        private async UniTask _notifyGameInitialize()
        {
            ProfilerMarker marker = new("_notifyGameInitialize");
            marker.Begin();

            foreach (IInitializeListener listener in _initListeners)
            {
                if (listener.IsInitialized)
                {
                    continue;
                }

                await listener.Initialize();

                listener.IsInitialized = true;
            }

            marker.End();

        }

        private UniTask _notifySubscribe()
        {
            ProfilerMarker marker = new("_notifySubscribe");
            marker.Begin();

            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.Subscribe();
            }

            marker.End();

            return UniTask.CompletedTask;
        }

        private async UniTask _notifyGameStart()
        {
            foreach (IStartListener listener in _startListeners)
            {
                await listener.GameStart();
            }
        }

        private void _notifyGameUpdate(float deltaTime)
        {
            foreach (IUpdateListener listener in _updateListeners)
            {
                listener.GameUpdate(deltaTime);
            }
        }

        private void _notifyGameFixedUpdate(float fixedDeltaTime)
        {
            foreach (IFixedUpdateListener listener in _fixedUpdateListeners)
            {
                listener.GameFixedUpdate(fixedDeltaTime);
            }
        }

        private void _notifyGameExit()
        {
            ProfilerMarker marker = new("_notifyGameExit");
            marker.Begin();

            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.Unsubscribe();
            }
            
            foreach (IExitListener listener in _exitListeners)
            {
                listener.GameExit();
            }
            
            marker.End();
        }
    }
}