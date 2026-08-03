using System;
using Core.Input;
using VContainer.Unity;

namespace Code.Game.World
{
    public class CameraFollow : IStartable, IDisposable
    {
        private readonly CameraView _cameraView;
        private readonly IInputService _inputService;


        public CameraFollow(CameraView cameraView, IInputService inputService)
        {
            _cameraView = cameraView;
            _inputService = inputService;
        }
        
        public void Start()
        {
            _inputService.ActionStarted += _inputActionStarted;
        }

        public void Dispose()
        {
            _inputService.ActionStarted += _inputActionStarted;
        }

        private void _inputActionStarted(EInputAction action)
        {
            if (action is not EInputAction.DoubleClick)
            {
                return;
            }
            
            _cameraView.MoveTo(_inputService.MouseWorldPosition);
        }
    }
}