using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Core.Scenes
{
    public class SceneLoadingCurtain : Essential.Mono, ISceneCurtain
    {
        private const float ANIMATION_DURATION = 1;
        
        [SerializeField] private CanvasGroup _canvasGroup;

        private Tween _tween;
        

        public async UniTask Show()
        {
            _canvasGroup.alpha = 0;
            
            gameObject.SetActive(true);
            
            _tween?.Kill();
            
            _tween = _canvasGroup.DOFade(1, ANIMATION_DURATION);
            
            await _tween.ToUniTask();
        }

        public async UniTask Hide()
        {
            _canvasGroup.alpha = 1;
            
            _tween?.Kill();
            
            _tween = _canvasGroup.DOFade(0, ANIMATION_DURATION);
            
            await _tween.ToUniTask();
            
            gameObject.SetActive(false);
        }
    }
}