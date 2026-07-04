using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.Scenes
{
    public class SceneService 
    {
        public event Action<EScene> SceneUnloaded;
        public event Action<EScene> SceneLoaded;

        private EScene _currentScene;
        
        
        public async UniTask LoadSceneAsync(EScene scene)
        {
            SceneUnloaded?.Invoke(_currentScene);
            
            await SceneManager.LoadSceneAsync(scene.ToString());

            _currentScene = scene;
            
            SceneLoaded?.Invoke(scene);
        }
    }
}