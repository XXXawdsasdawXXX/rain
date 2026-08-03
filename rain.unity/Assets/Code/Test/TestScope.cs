using Code.Test.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Test
{
    public class TestScope : LifetimeScope
    {
        [SerializeField] private QAWindowView _qaWindow;
        
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_qaWindow).AsSelf();

            builder.Register<QAWindowController>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
