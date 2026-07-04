using System;
using Essential.Common.Data;
using UnityEngine;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace Core.Input
{
    [Preserve]
    public sealed class InputService : ITickable, IDisposable
    {
        public event Action<EInputAction> ActionStarted;
        public event Action<EInputAction> ActionEnded;

        public ReactiveProperty<Vector2> Direction { get; } = new(Vector2.zero);
        public Vector3 MousePosition { get; private set; }

        
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

        public void Tick()
        {
            Direction.Value = new Vector2(
                UnityEngine.Input.GetAxisRaw("Horizontal"),
                UnityEngine.Input.GetAxisRaw("Vertical"));

            MousePosition = UnityEngine.Input.mousePosition;
            
            foreach (InputActionKey inputActionKey in _inputActionKeys)
            {
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
            Direction.Dispose();
        }
    }
}