using System;
using UnityEngine;

namespace Code.Game.World.Characters
{
    [Serializable]
    public class WorldArmorPieceView : AArmorPieceView<SpriteRenderer>
    {
        public WorldArmorPieceView()
        {
        }

        public WorldArmorPieceView(EBodyPart part, SpriteRenderer[] renderers) : base(part, renderers)
        {
        }
        
        protected override void SetSprite(SpriteRenderer renderer, Sprite sprite)
        {
            renderer.sprite = sprite;
        }

        protected override Sprite GetSprite(SpriteRenderer renderer)
        {
            return renderer.sprite;
        }
    }
}