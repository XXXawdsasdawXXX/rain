using Core.AssetManagement;
using Core.GameLoop;
using Core.Libraries;
using Core.Libraries.Assets;
using Core.Localization;
using Core.Save;
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
        private readonly AssetManagement.Installers _installers;
        private readonly Configurations _configurations;

        public BootstrapState(GameStateMachine gameStateMachine)
        {
        }

        public BootstrapState(GameStateMachine gameStateMachine, AssetManagement.Installers installers, Configurations configurations)
        {
            _gameStateMachine = gameStateMachine;
            _installers = installers;
            _configurations = configurations;
        }

        public async UniTask Initialize()
        {
            Container container = InitializeProjectContext();

            await container.GetService<LocalizationService>().Initialize();
            container.GetService<SaveService>().Initialize().Forget();
            container.GetService<GameEventDispatcher>().Initialize();
            
            LoadGame(container);
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

        private static void LoadGame(Container container)
        {
            SaveService saveService = container.GetService<SaveService>();
            GameModel model = container.GetService<GameModel>();

            GameModel loadedModel = saveService.LoadLastGameModel();

            model.CopyFrom(loadedModel);
        }

        public UniTask Enter()
        {
            _gameStateMachine.SwitchState(typeof(MainMenuState));

            return UniTask.CompletedTask;
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}