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
        
        // на LeftClick down:
        // candidate = true
        // pressScreen = mouseScreen
        //     pressTime = now
        // anchorWorld = mouseWorld   // пока кандидат
        //     каждый кадр:
        // если candidate и не panning:
        // если время > порог ИЛИ |mouseScreen - pressScreen| > порогПикселей:
        // panning = true
        // anchorWorld = mouseWorld   // зафиксировать захват
        //     если panning:
        // delta = anchorWorld - mouseWorld
        //     desired = cameraPos + delta
        // desired.z = cameraZ
        //     на LeftClick up:
        // candidate = false
        // panning = false
        // в LateTick:
        // если panning:
        // cameraPos = desired
        private const float DELEY_DURATION = 0.3f;
        private const float SPEED = 7.2f;

        private readonly CameraView _cameraView;
        private readonly IInputService _input;

        private float _startPressTime;
        private Vector3 _startPosition;
        private Vector3 _direction;


        private bool _isPressed;


        public CameraFollow(CameraView cameraView, IInputService input)
        {
            _cameraView = cameraView;
            _input = input;
        }

        public void Start()
        {
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
            if (_isPressed)
            {
                Vector3 target = (_startPosition - _input.MouseWorldPosition);
                _direction = Vector3.Lerp(_cameraView.transform.position, target, SPEED * Time.deltaTime);
                _direction.z = -10;
            }
        }

        public void LateTick()
        {
            _cameraView.transform.position = _direction;
        }
    }
}