using Core.Audio.AudioSystem;
using Core.Input;
using VContainer;
using VContainer.Unity;

namespace Core.DI
{
    public class ProjectScope : LifetimeScope
    {
        

    
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AudioService>(Lifetime.Singleton).AsImplementedInterfaces();

        }
    }
}