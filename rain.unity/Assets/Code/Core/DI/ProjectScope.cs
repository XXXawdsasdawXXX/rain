using Core.Audio.AudioSystem;
using Core.Input;
using Core.Scenes;
using Core.StateMachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.DI
{
    public class ProjectScope : LifetimeScope
    {
        [SerializeField] private SceneLoadingCurtain _loadingCurtain;
        
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AudioService>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.RegisterInstance<ISceneCurtain>(_loadingCurtain);
            
            builder.RegisterEntryPoint<GameStateMachine>();
        }
    }
}