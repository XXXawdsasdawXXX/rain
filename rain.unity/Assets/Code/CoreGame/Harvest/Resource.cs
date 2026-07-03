using System;
using Code.CoreGame.Common.Collisions;
using Code.CoreGame.Harvest.Data;
using Core.Common.Extensions;
using Core.GameLoop;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace Code.CoreGame.Harvest
{
    [Serializable]
    public class Resource : Essential.Mono, IStartListener, ISubscriber
    {
        public event Action<Resource> HarvestStarted;
        public event Action<Resource> Changed;
        [field: SerializeField] public EResource Type { get; private set; }
        [field: SerializeField] public float2 Position { get; private set; }
        [field: SerializeField] public ResourceConfig Config { get; private set; }
        public bool IsMax => CurrentValue >= Config.MaxValue;
        public bool IsEnded => CurrentValue <= 0;
        public int CurrentValue => _current;
        
        private int _current;

        [SerializeField] private InteractionTrigger _interactionTrigger;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        
        public UniTask GameStart()
        {
            _current = Config.MaxValue;
            
            return UniTask.CompletedTask;
        }

        public void Subscribe()
        {
            _interactionTrigger.InteractionPerformed += _onInteractionPerformed;
        }

        public void Unsubscribe()
        {
            _interactionTrigger.InteractionPerformed -= _onInteractionPerformed;
        }

        public void SetValue(int value)
        {
            _current = value;
            
            Changed?.Invoke(this);
        }

        public void UpdateValue(int value)
        {
            _current += value;

            if (_current > Config.MaxValue)
            {
                _current = Config.MaxValue;
            }
            else if (_current < 0)
            {
                _current = 0;
            }
            
            Changed?.Invoke(this);
        }

        private void _onInteractionPerformed()
        {
            HarvestStarted?.Invoke(this);
        }


#if UNITY_EDITOR

        public void Validate(ResourceConfig resourceConfig)
        {
            Position = transform.position.AsFloat2();
            
            Config = resourceConfig;
         
            _spriteRenderer.sprite = resourceConfig.WorldView;

            gameObject.name = $"resource_{Type,10} [{Position.x,5}:{Position.x,5}]";
        }
#endif
   
    }
}