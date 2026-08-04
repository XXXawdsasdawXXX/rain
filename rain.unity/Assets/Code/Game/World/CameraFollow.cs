using System;
using Core.Input;
using JetBrains.Annotations;
using UnityEngine;
using VContainer.Unity;

namespace Code.Game.World
{
    [UsedImplicitly]
    public class CameraFollow : IStartable, IDisposable, ITickable, ILateTickable
    {
        private const float DELEY_DURATION = 0.15f;
        private const float BLEND = 4.8f;

        private readonly CameraView _cameraView;
        private readonly IInputService _input;

        private float _startPressTime;
        private Vector3 _startPosition;
        private Vector3 _target;

        private bool _isPressed;


        public CameraFollow(CameraView cameraView, IInputService input)
        {
            _cameraView = cameraView;
            _input = input;
        }

        public void Start()
        {
            _target = _cameraView.transform.position;
            
            _input.ActionStarted += _inputActionStarted;
            _input.ActionEnded += _inputActionEnded;
        }

        public void Dispose()
        {
            _input.ActionStarted -= _inputActionStarted;
            _input.ActionEnded -= _inputActionEnded;
        }

        private void _inputActionStarted(EInputAction action)
        {
            if (action == EInputAction.LeftClick)
            {
                _startPressTime = Time.unscaledTime;
                _startPosition = _input.MouseWorldPosition;
                _isPressed = true;
            }
        }

        private void _inputActionEnded(EInputAction action)
        {
            if (action == EInputAction.LeftClick)
            {
                _isPressed = false;
            }
        }

        public void Tick()
        {
            if (_isPressed && Time.unscaledTime - _startPressTime > DELEY_DURATION)
            {
                _target = _cameraView.transform.position + (_startPosition - _input.MouseWorldPosition);
                _target.z = -10;
            }
        }

        public void LateTick()
        {
            _cameraView.transform.position = Vector3.Lerp(_cameraView.transform.position, _target, BLEND * Time.deltaTime);
        }
    }
}