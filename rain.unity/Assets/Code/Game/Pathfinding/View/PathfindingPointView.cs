using Essential;
using UnityEngine;

namespace Code.Pathfinding
{
    public class PathfindingPointView : Essential.Mono, IPoolEntity
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }
        
        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}