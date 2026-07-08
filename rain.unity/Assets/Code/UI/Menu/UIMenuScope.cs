using Code.UI.Components.Window;
using TriInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Code.UI.Menu
{
    public sealed class UIMenuScope : LifetimeScope
    {
        [SerializeField] private AUIWindowView[] _views;

        protected override void Configure(IContainerBuilder builder)
        {
            foreach (AUIWindowView windowView in _views)
            {
                builder.RegisterInstance(windowView).AsSelf();
            }

            builder.Register<MenuNavigator>(Lifetime.Singleton);
            
            builder.Register<GameWindowController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<NewGameWindowController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<DeleteGameWindowController>(Lifetime.Singleton).AsImplementedInterfaces();
        }

#if UNITY_EDITOR
        [Button]
        private void _findViews()
        {
            _views = GetComponentsInChildren<AUIWindowView>(true);
            EditorUtility.SetDirty(this);
        }
#endif
    }
}