using Core.AssetManagement;
using Core.AssetManagement.Libraries.Assets;
using Core.GameLoop;
using Core.Localization;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

namespace Core.StateMachine.States
{
    public class BootstrapState : IState
    {
        public bool IsInitialized { get; set; }

        private readonly GameStateMachine _gameStateMachine;
        private readonly Installers _installers;
        private readonly Configurations _configurations;

        public BootstrapState(GameStateMachine gameStateMachine, Installers installers, Configurations configurations)
        {
            _gameStateMachine = gameStateMachine;
            _installers = installers;
            _configurations = configurations;
        }

        public async UniTask Initialize()
        {
            Container container = InitializeProjectContext();
            
            await container.GetService<LocalizationService>().Initialize();
            await container.GetService<GameEventDispatcher>().Register(Container.Instance.GetGameListeners());
        }

        private Container InitializeProjectContext()
        {
            AssetLibrary assetLibrary = _configurations.Get<AssetLibrary>();

            AssetProvider.Instantiate(assetLibrary.SceneComponents.Get(AssetKey.CAMERA));

            if (Profiler.enabled)
            {
                AssetProvider.Instantiate(assetLibrary.UICanvases.Get(AssetKey.CANVAS_PROFILER));
            }

            ContextEntities projectContext = ContextBuilder.BuildContext(_installers.ProjectsInstaller.GetTypes());
            projectContext.Services.Add(typeof(GameStateMachine), _gameStateMachine);
            
            Container container = new(projectContext);
            
            container.AddConfig(_installers);
            
            foreach (ScriptableObject config in _configurations.Configs)
            {
                container.AddConfig(config);
            }

            return container;
        }
        
        public async UniTask Enter()
        {
            await _gameStateMachine.SwitchState(typeof(MainMenuState));
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}