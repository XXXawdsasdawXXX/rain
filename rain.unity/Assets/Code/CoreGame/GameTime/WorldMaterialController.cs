using Core.AssetManagement.Libraries.Assets;
using Core.Common.Data;
using Core.GameLoop;
using Core.ServiceLocator;
using Core.ServiceLocator.MarkerInterfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.CoreGame.GameTime
{
    public class WorldMaterialController : IMono, IInitializeListener, IUpdateListener, IExitListener
    {
        private static readonly int OVERLAY_BLEND = Shader.PropertyToID(MATERIAL_PARAM_NAME);

        private const string MATERIAL_PARAM_NAME = "_OverlayBlend";
        private const float MAX_VALUE = 0.675f;

        public bool IsInitialized { get; set; }
        
        private Cache<int> _lastUpdatedMinute;
        private Material _worldMaterial;
        private GameTime _gameTime;
        
        
        public UniTask Initialize()
        {
            _worldMaterial = Container.Instance.GetSO<AssetLibrary>().Material.Get(MaterialLibrary.WORLD);
            _gameTime = Container.Instance.GetService<GameTime>();
          
            _lastUpdatedMinute = new Cache<int>();
            
            return UniTask.CompletedTask;
        }

        public void GameUpdate(float deltaTime)
        {
            if (_lastUpdatedMinute.Update(_gameTime.Current.Hours * 60 + _gameTime.Current.Minutes))
            {
                float timeOfDay = (_gameTime.Current.Hours * 60 + _gameTime.Current.Minutes) / 1440f;
                float shiftedTime = (timeOfDay - 0.2f) * 2 * Mathf.PI;
    
                float brightness = (Mathf.Cos(shiftedTime) + 1) / 2 * MAX_VALUE;
    
                _worldMaterial.SetFloat(OVERLAY_BLEND, brightness);
            }
        }

        public void GameExit()
        {
            _worldMaterial.SetFloat(OVERLAY_BLEND, 0);
        }
    }
}