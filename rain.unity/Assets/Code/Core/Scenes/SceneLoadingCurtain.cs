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
        private bool _isShowing;

        
        public  UniTask Show()
        {
            if (_canvasGroup == null || _isShowing)
            {
                return UniTask.CompletedTask;
            }

            _isShowing = true;
   
            /*_canvasGroup.alpha = 0;
            gameObject.SetActive(true);
            
            _tween?.Kill();
            _tween = _canvasGroup.DOFade(1, ANIMATION_DURATION)
                .SetLink(_canvasGroup.gameObject, LinkBehaviour.KillOnDisable);*/
            
            _canvasGroup.alpha = 1;
            
            return UniTask.CompletedTask;
            
            //await _tween.ToUniTask();
        }

        public void ShowWithOutAnimation()
        {
            _canvasGroup.alpha = 1;
            
            gameObject.SetActive(true);
            
            _isShowing = true;
        }

        public UniTask Hide()
        {
            if (_canvasGroup == null || !_isShowing)
            {
                return UniTask.CompletedTask;
            }

            _isShowing = false;
            // _canvasGroup.alpha = 1;
            //
            // _tween?.Kill();
            // _tween = _canvasGroup.DOFade(0, ANIMATION_DURATION)
            //     .SetLink(_canvasGroup.gameObject, LinkBehaviour.KillOnDisable);
            // await _tween.ToUniTask();

            _canvasGroup.alpha = 0;
            
            gameObject.SetActive(false);
            
            return UniTask.CompletedTask;
        }
    }
}