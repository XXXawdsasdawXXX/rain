using System;
using System.Linq;
using Core.Scenes;
using Core.ServiceLocator;
using Essential;
using UnityEngine;

namespace Core.AssetManagement
{
    [CreateAssetMenu(fileName = "Storage_Installer", menuName = "Game/Storage/Installers")]
    public class Installers : ScriptableObject
    {
        [Serializable]
        private struct SceneInstaller
        {
            public EScene Scene;
            public Installer Installer;
        }
        
        [field: SerializeField] public Installer ProjectsInstaller { get; private set; }
        [field: SerializeField] public Installer CoreGameInstaller { get; private set; }

        
        [SerializeField] private SceneInstaller[] _sceneInstallers;
        
        
        public Installer GetSceneInstaller(EScene scene)
        {
            Installer installer = _sceneInstallers.FirstOrDefault(i => i.Scene == scene).Installer;

            if (installer == null)
            {
                Log.Error(this, $"GetSceneInstaller - not find installer for scene {scene}");
            }
            return installer;
        }
    }
}