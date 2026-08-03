using DG.Tweening;
using UnityEngine;

namespace Core.Input
{
    public class CameraView : Essential.Mono
    {
        private const float SPEED = 2;
        private const float Z_POS = -10;
        
        [field: SerializeField] public Camera Camera { get; private set; }

        private Tween _tween;
        
        public void MoveTo(Vector3 worldPosition)
        {
            _tween?.Kill();
            
            worldPosition.z = Z_POS;

            _tween = Camera.transform.DOMove(worldPosition, SPEED)
                .SetSpeedBased()
                .SetEase(Ease.Linear);
        }
    }
}