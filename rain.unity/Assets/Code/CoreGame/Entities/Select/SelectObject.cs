using System;
using System.Linq;
using System.Threading;
using Code.CoreGame.Entities.Select.Impact;
using Cysharp.Threading.Tasks;
using Essential;
using UnityEngine;

namespace Code.CoreGame.Entities.Select
{
    public sealed class SelectObject : Essential.Mono
    {
        public enum EType
        {
            None, 
            Hero,
            Resource,
            Item
        }
        
        
        [field: SerializeField]
        public EType Type { get; private set; }
        
        [SerializeField]
        private float _unHoverDelay = 1.4f;
        

        [SerializeReference] 
        private SelectableImpact[] _impacts;

        private CancellationTokenSource _cts;
        private bool _isHover;
        
        
        public async void Hover(bool isHover)
        {
            if (_isHover == isHover)
            {
                return;
            }
            
            _cts?.Dispose();

            if (!isHover)
            {
                _cts = new CancellationTokenSource();
                await UniTask.Delay(TimeSpan.FromSeconds(_unHoverDelay), cancellationToken: _cts.Token);
                await UniTask.WaitUntil(() => _impacts.All(i => i.CanUnHover()), cancellationToken: _cts.Token);
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: _cts.Token);
            }
            
            foreach (SelectableImpact selectableImpact in _impacts)
            {
                selectableImpact.Hovered(isHover);
            }

            _isHover = isHover;
            
            Log.Info($"{gameObject.name} is hover {isHover}");
        }

        public void Press()
        {
            if (!_isHover)
            {
                return;
            }
            
            foreach (SelectableImpact selectableImpact in _impacts)
            {
                selectableImpact.Pressed();
            }
            
            Log.Info($"{gameObject.name} pressed");
        }
    }
}