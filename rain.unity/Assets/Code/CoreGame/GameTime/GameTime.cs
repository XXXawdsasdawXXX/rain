using System;
using Core.GameLoop;
using Core.Save;
using Core.Save.Models;
using Core.ServiceLocator;
using Core.ServiceLocator.MarkerInterfaces;
using Cysharp.Threading.Tasks;
using UnityEngine.Scripting;

namespace Code.CoreGame.GameTime
{
    [Preserve]
    public class GameTime : IService, IInitializeListener, IUpdateListener
    {
        public bool IsInitialized { get; set; }
        public TimeSpan Current => _gameModel.World.GameTime;

        private GameModel _gameModel;
        private float _timeScale;
        private double _lastUpdateTime;

        
        public UniTask Initialize()
        {
            _timeScale = Container.Instance.GetSO<GameTimeSettings>().TimeScale;

            _gameModel = Container.Instance.GetService<GamesSaver>().Current;
            
            return UniTask.CompletedTask;;
        }
        
        public void GameUpdate(float deltaTime)
        {
            _updateTime(deltaTime);
        }

        private void _updateTime(float deltaTime)
        {
            _gameModel.World.GameTime += TimeSpan.FromSeconds(deltaTime * _timeScale);

            if (UnityEngine.Time.time - _lastUpdateTime >= 1.0f)
            {
                 _gameModel.World.GameTime += TimeSpan.FromSeconds(deltaTime * _timeScale);
                _lastUpdateTime = UnityEngine.Time.time;
            }
        }
    }
}
