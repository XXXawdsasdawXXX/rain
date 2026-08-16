using System;
using Code.Game.Characters;
using Code.Game.Characters.Armor;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Components
{
    [Serializable]
    public class UIArmorPieceView : AArmorPieceView<Image>
    {
        public UIArmorPieceView()
        {
        }

        public UIArmorPieceView(EBodyPart part, Image[] renderers) : base(part, renderers)
        {
        }

        protected override void SetSprite(Image renderer, Sprite sprite)
        {
            renderer.sprite = sprite;
            renderer.color = sprite == null ? Color.clear : Color.white;
        }

        protected override Sprite GetSprite(Image renderer)
        {
            return renderer.sprite;
        }
    }

    [Serializable]
    public class UIArmorRender : AArmorRender<Image, UIArmorPieceView>
    {
        /*
        Mask = 0,
        Head = 1,
        Torso = 2,
        Pants = 3,
        Shoes = 4,
        */

        protected override UIArmorPieceView[] CreateDefaultPieces()
        {
            return new[]
            {
                CreatePiece(EBodyPart.Mask, 1),
                CreatePiece(EBodyPart.Head, 1),
                CreatePiece(EBodyPart.Torso, 1),
            };
        }
    }
}
