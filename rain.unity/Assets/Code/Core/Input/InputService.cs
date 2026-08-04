using System;
using Essential;
using UnityEngine;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace Core.Input
{
    public interface IInputService
    {
        const float HOLDDELEY_DURATION = 0.15f;
        const float DOUBLE_CLICK_TIME = 0.6f;
        public event Action<EInputAction> ActionStarted;
        public event Action<EInputAction> ActionEnded;
        public Vector3 MouseWorldPosition { get; }
        public float StartLeftClickTime { get; }
    }

    [Preserve]
    public sealed class InputService : IInputService, ITickable
    {
        public event Action<EInputAction> ActionStarted;
        public event Action<EInputAction> ActionEnded;
        public Vector3 MouseWorldPosition { get; private set; }
        public float StartLeftClickTime { get; private set; }

        private readonly Camera _camera;

        private bool _isPressLeftClick;
        
        private readonly InputActionKey[] _inputActionKeys = 
        {
            new()
            {
                Key = KeyCode.F,
                Action = EInputAction.Interaction
            },
            new()
            {
                Key = KeyCode.Mouse0,
                Action = EInputAction.LeftClick
            },
            new()
            {
                Key = KeyCode.Mouse1,
                Action = EInputAction.RightClick
            },
            new()
            {
                Key = KeyCode.Escape,
                Action = EInputAction.Esc
            }
        };
        
        
        
        public InputService(CameraView cameraView)
        {
            _camera = cameraView.Camera;
        }

        public void Tick()
        {
            _updateMousePosition();

            foreach (InputActionKey inputActionKey in _inputActionKeys)
            {
                if (inputActionKey.Action is EInputAction.LeftClick)
                {
                    if (UIUtils.IsPointerOverUI())
                    {
                        continue;
                    }
                }
                
                if (_wasActionPressed(inputActionKey))
                {
                    ActionStarted?.Invoke(inputActionKey.Action);

                    if (inputActionKey.Action is EInputAction.LeftClick)
                    {
                        float time = Time.unscaledTime;

                        if (time - StartLeftClickTime < IInputService.DOUBLE_CLICK_TIME)
                        {
                            ActionStarted?.Invoke(EInputAction.DoubleClick);

                            Log.Info(this, "invoke double click");
                        }

                        _isPressLeftClick = true;
                        StartLeftClickTime = time;
                    }
                }
                
                if (_wasActionReleased(inputActionKey))
                {
                    ActionEnded?.Invoke(inputActionKey.Action);

                    if (inputActionKey.Action is EInputAction.LeftClick)
                    {
                        _isPressLeftClick = false;
                    }
                }
            }
        }
        
        private static bool _wasActionPressed(InputActionKey inputActionKey)
        {
            return inputActionKey.Key switch
            {
                KeyCode.Mouse0 => UnityEngine.Input.GetMouseButtonDown(0),
                KeyCode.Mouse1 => UnityEngine.Input.GetMouseButtonDown(1),
                _ => UnityEngine.Input.GetKeyDown(inputActionKey.Key)
            };
        }

        private static bool _wasActionReleased(InputActionKey inputActionKey)
        {
            return inputActionKey.Key switch
            {
                KeyCode.Mouse0 => UnityEngine.Input.GetMouseButtonUp(0),
                KeyCode.Mouse1 => UnityEngine.Input.GetMouseButtonUp(1),
                _ => UnityEngine.Input.GetKeyUp(inputActionKey.Key)
            };
        }

        private void _updateMousePosition()
        {
            Vector3 mousePosition = UnityEngine.Input.mousePosition;
            Vector3 screenPos = mousePosition;
            screenPos.z = Mathf.Abs(_camera.transform.position.z);
            Vector3 worldPoint = _camera.ScreenToWorldPoint(screenPos);
            worldPoint.z = 0f;
            MouseWorldPosition = worldPoint;
        }
    }
}