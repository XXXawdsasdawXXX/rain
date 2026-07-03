using System.Collections.Generic;
using Core.AssetManagement;
using Core.AssetManagement.Libraries.Assets;
using Core.GameLoop;
using Core.Scenes;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using Essential;
using UnityEngine;

namespace Core.StateMachine.States
{
    public class CoreGameState : IState
    {
        public bool IsInitialized { get; set; }

        private AssetManagement.Installers _installers;
        private AssetLibrary _assetLibrary;

        private SceneService _sceneService;
        private GameEventDispatcher _gameEventDispatcher;

        private readonly List<GameObject> _coreEntities = new();


        public UniTask Initialize()
        {
            _sceneService = Container.Instance.GetService<SceneService>();
            _gameEventDispatcher = Container.Instance.GetService<GameEventDispatcher>();

            _installers = Container.Instance.GetSO<Installers>();
            _assetLibrary = Container.Instance.GetSO<AssetLibrary>();

            return UniTask.CompletedTask;
        }

        public async UniTask Enter()
        {
            await _sceneService.LoadSceneAsync(EScene.Game); //todo use player progress
            
            _createHUD();
            
            if (_installers.CoreGameInstaller == null)
            {
                Log.Error(this, $"enter -> installer for core game is null");
                return;
            }

            Container.Instance.Context.BuildChildContext(_installers.CoreGameInstaller.GetTypes());
            await _gameEventDispatcher.Register(Container.Instance.GetGameListeners());

            SubscribeToEvents(true);
        }

        private void _createHUD()
        {
            GameObject canvasPrefab = _assetLibrary.UICanvases.Get(AssetKey.CANVAS_CORE_GAME);
            GameObject canvasInstance = AssetProvider.Instantiate(canvasPrefab);
            _coreEntities.Add(canvasInstance);
        }

        public UniTask Exit()
        {
            _gameEventDispatcher.Dispose();
            
            SubscribeToEvents(false);

            foreach (GameObject entity in _coreEntities)
            {
                Object.Destroy(entity);
            }

            _coreEntities.Clear();

            return UniTask.CompletedTask;
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _sceneService.SceneUnloaded += OnSwitchScene;
                _sceneService.SceneLoaded += OnSwitchScene;
            }
            else
            {
                _sceneService.SceneUnloaded -= OnSwitchScene;
                _sceneService.SceneLoaded -= OnSwitchScene;
            }
        }

        private async void OnSwitchScene(EScene obj)
        {
            _gameEventDispatcher.Dispose();

            Container.Instance.Context.Child.BuildChildContext(_installers.GetSceneInstaller(obj)?.GetTypes());

            await _gameEventDispatcher.Register(Container.Instance.GetGameListeners());
        }
    }
}