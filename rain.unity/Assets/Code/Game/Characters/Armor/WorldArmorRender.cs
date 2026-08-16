using System;
using Code.Game.Characters.Armor;
using UnityEngine;

namespace Code.Game.Characters
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

    [Serializable]
    public class WorldArmorRender : AArmorRender<SpriteRenderer, WorldArmorPieceView>
    {
        /*
        Mask = 0,
        Head = 1,
        Torso = 2,
        Pants = 3,
        Shoes = 4,
        */

        public WorldArmorRender() : base()
        {
            
        }
        
        protected override WorldArmorPieceView[] CreateDefaultPieces()
        {
            return new[]
            {
                CreatePiece(EBodyPart.Mask, 1),
                CreatePiece(EBodyPart.Head, 1),
                CreatePiece(EBodyPart.Torso, 3), // 0 - main body | 1 - left hand | 2 - right hand
                CreatePiece(EBodyPart.Pants, 2), // 0 - left | 1 - right
                CreatePiece(EBodyPart.Shoes, 2), // 0 - left | 1 - right
            };
        }
    }
}
