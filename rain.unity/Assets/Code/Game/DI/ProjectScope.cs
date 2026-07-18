using Code.Game.Save;
using Code.Game.StateMachine;
using Core.AssetManagement.Libraries.Assets;
using Core.Audio.AudioSystem;
using Core.Input;
using Core.Save;
using Core.Scenes;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Game.DI
{
    public class ProjectScope : LifetimeScope
    {
        [SerializeField] private CameraView _cameraView;
        [SerializeField] private SceneLoadingCurtain _loadingCurtain;
        [SerializeField] private AssetLibrary _assetLibrary;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance<ISceneCurtain>(_loadingCurtain);
            builder.RegisterInstance(_cameraView);
            
            //save
            builder.Register<ISaveProcessor, FileSaveProcessor>(Lifetime.Singleton);
            builder.Register<SettingsSaveRepository>(Lifetime.Singleton);
            builder.Register<GameSaveRepository>(Lifetime.Singleton);

            //configurations
            builder.RegisterInstance(_assetLibrary).AsSelf();
            
            //services
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AudioService>(Lifetime.Singleton).AsImplementedInterfaces();
            
            //entry point
            builder.RegisterEntryPoint<GameStateMachine>().AsSelf();
        }
    }
}