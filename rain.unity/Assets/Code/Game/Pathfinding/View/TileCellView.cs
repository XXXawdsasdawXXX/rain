using Code.Game.World;
using Essential;
using UnityEngine;

namespace Code.Game.Pathfinding.View
{
    public class TileCellView : WorldMono, IPoolEntity
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