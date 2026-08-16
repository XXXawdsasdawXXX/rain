using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TriInspector;
using UnityEngine;

namespace Essential.Data
{
    [Serializable]
    public class ReactiveProperty<T>
    {
        [JsonIgnore]
        public T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;
                    _changed?.Invoke(_value);
                }
            }
        }
        
        private event Action<T> _changed;
        
        [SerializeField, ReadOnly] private T _value;

        
        public ReactiveProperty(T value)
        {
            _value = value;
        }

        public void SubscribeProperty(Action<T> action)
        {
            _changed += action;
        }
        
        public void UnsubscribeProperty(Action<T> action)
        {
            _changed -= action;
        }

        public void Dispose()
        {
            _changed = null;
            _value = default;
        }

        public void ClearSubscription()
        {
            _changed = null;
        }

        public void SetValueWithoutNotify(T value)
        {
            _value = value;
        }
    }
}