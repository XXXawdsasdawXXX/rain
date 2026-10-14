using System;
using Code.Game.Characters.Armor;
using UnityEngine;

namespace Code.Game.Characters
{
    [Serializable]
    public class WorldArmorRender : AArmorRender<SpriteRenderer, WorldArmorPieceView>
    {
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
