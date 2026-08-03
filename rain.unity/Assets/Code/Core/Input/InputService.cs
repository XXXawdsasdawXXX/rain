using System;
using Essential;
using Essential.Data;
using UnityEngine;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace Core.Input
{
    public interface IInputService
    {
        public event Action<EInputAction> ActionStarted;
        public event Action<EInputAction> ActionEnded;
        public Vector3 MouseWorldPosition { get; }
    }

    [Preserve]
    public sealed class InputService : IInputService, ITickable, IDisposable
    {
        public event Action<EInputAction> ActionStarted;
        public event Action<EInputAction> ActionEnded;
        public Vector3 MouseWorldPosition { get; set; }
        
        private readonly ReactiveProperty<Vector2> _direction = new(Vector2.zero);

        private readonly Camera _camera;
        
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
            _direction.Value = new Vector2(
                UnityEngine.Input.GetAxisRaw("Horizontal"),
                UnityEngine.Input.GetAxisRaw("Vertical"));

            _updateMousePosition();

            foreach (InputActionKey inputActionKey in _inputActionKeys)
            {
                if (inputActionKey.Action is EInputAction.LeftClick && UIUtils.IsPointerOverUI())
                {
                    continue;
                }
                
                if (_wasActionPressed(inputActionKey))
                {
                    ActionStarted?.Invoke(inputActionKey.Action);
                }
                
                if (_wasActionReleased(inputActionKey))
                {
                    ActionEnded?.Invoke(inputActionKey.Action);
                }
            }
        }

        public void Dispose()
        {
            _direction.Dispose();
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
            Vector3 screenPos = UnityEngine.Input.mousePosition;
            screenPos.z = Mathf.Abs(_camera.transform.position.z);
            Vector3 worldPoint = _camera.ScreenToWorldPoint(screenPos);
            worldPoint.z = 0f;
            MouseWorldPosition = worldPoint;
        }
    }
}