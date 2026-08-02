using Code.UI.Components.Window;
using Code.UI.Hud.Windows;
using Code.UI.Hud.Windows.Heroes;
using TriInspector;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.UI.Hud
{
    public class UIHudScope : LifetimeScope
    {
        [SerializeField] private AUIWindowView[] _views;
        
        protected override void Configure(IContainerBuilder builder)
        {
            foreach (AUIWindowView windowView in _views)
            {
                builder.RegisterInstance(windowView).AsSelf();
            }

            builder.Register<HudNavigator>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<HeroesListWindowController>(Lifetime.Singleton).AsImplementedInterfaces();
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