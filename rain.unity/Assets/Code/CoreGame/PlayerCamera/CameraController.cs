using Core.GameLoop;
using Core.ServiceLocator;
using Core.ServiceLocator.MarkerInterfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.CoreGame.PlayerCamera
{
    public class CameraController : IMono, IInitializeListener, IStartListener, IUpdateListener
    {
        public bool IsInitialized { get; set; }
        public UniTask Initialize()
        {
            _cameraView = Container.Instance.GetView<CameraView>();
            
            return UniTask.CompletedTask;
        }

        
        private CameraView _cameraView;
        

        public UniTask GameStart()
        {
         
            return UniTask.CompletedTask;
        }

        public void GameUpdate(float deltaTime)
        {
            //todo set hero value
            /*if (_cameraView != null && _userProvider.Hero != null)
            {
                Vector3 position = _userProvider.Hero.transform.position;

                position.z = -10;
                
                _cameraView.transform.position = position;
            }
            else
            {
            }*/
        }
    }
}