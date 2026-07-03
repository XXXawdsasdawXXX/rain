using Code.CoreGame.Entities.Characters.Hero.Data;
using Core.GameLoop;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Plugins.Demigiant.DOTween.Modules;
using UnityEngine;

namespace Code.CoreGame.Entities.Characters.Hero.Components
{
    public class HeroColor : Essential.Mono, IInitializeListener, ISubscriber, IExitListener
    {
        [SerializeField] private CurrentMaxValue _currentMaxValue;
        [SerializeField] private SpriteRenderer[] _spriteRenderers;

        private ColorTweenData _damageTween;

        private Sequence _sequence;
        
        public bool IsInitialized { get; set; }
        public UniTask Initialize()
        { 
            _damageTween = Container.Instance.GetSO<HeroSettings>().DamageTween;
            
            return UniTask.CompletedTask;
        }

        public void Subscribe()
        {
            _currentMaxValue.Decreased += _playDamageTween;
        }

        public void Unsubscribe()
        {
            _currentMaxValue.Decreased -= _playDamageTween;
        }
        
        public void GameExit()
        {
            _sequence?.Kill();
        }

        private void _playDamageTween()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
            {
                _sequence.Join(spriteRenderer
                    .DOColor(_damageTween.Color, _damageTween.Duration)
                    .SetLoops(2, LoopType.Yoyo));
            }
        }
    }
}