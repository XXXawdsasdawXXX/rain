using System;
using System.Linq;
using Code.Game.Characters.Armor;
using Essential;
using TriInspector;
using UnityEngine;

namespace Code.Game.Characters
{
    [Serializable]
    public class ArmorPieceRender
    {
        [field: SerializeField, GUIColor(0.4f, 0.8f, 0.5f, 0.9f)] public EBodyPart Part { get; private set; }
        [field: SerializeField] public SpriteRenderer[] Renderers { get; private set; }

        public ArmorPieceRender(EBodyPart part, SpriteRenderer[] renderers)
        {
            Part = part;
            Renderers = renderers;
        }
    }
    
    
    [Serializable]
    public class ArmorRender
    {
        /*
        Mask = 0,
        Head = 1,
        Torso = 2,
        Pants = 3,
        Shoes = 4,
        */

        [SerializeField] private ArmorPieceRender[] _render = 
        {
            new(EBodyPart.Mask,  new SpriteRenderer[1]),
            new(EBodyPart.Head,  new SpriteRenderer[1]),
            new(EBodyPart.Torso, new SpriteRenderer[3]),// 0 - main body | 1 - left hand | 2 - right hand
            new(EBodyPart.Pants, new SpriteRenderer[2]),// 0 - left | 1 - right
            new(EBodyPart.Shoes, new SpriteRenderer[2]),// 0 - left | 1 - right
        };

        
        public void SetArmorPiece(ArmorPieceModel model)
        {
            ArmorPieceRender pieceRender = _render[(int)model.Part];
            
            for (int i = 0; i < pieceRender.Renderers.Length; i++)
            {
                SpriteRenderer renderer = pieceRender.Renderers[i];
            
                if (renderer == null)
                {
                    break;
                }

                renderer.sprite = model.Views  != null && model.Views.Length > i 
                    ? model.Views[i] 
                    : null;
            }
        }

        public Sprite[] GetCurrentArmorSprites(EBodyPart bodyPart)
        {
            SpriteRenderer[] renderers = _render[(int)bodyPart].Renderers;
            return (from r in renderers select r.sprite).ToArray();
        }
        
        [Button]
        public void SetArmorSet(ArmorSetModel armorSetModel)
        {
            if (armorSetModel == null)
            {
                //todo set default view

                Log.Error(this, "[SetArmorSet] set model is null");
                return;
            }
            
            foreach (ArmorPieceModel armorPieceModel in armorSetModel.Pieces)
            {
                SetArmorPiece(armorPieceModel);
            }
        }
    }
}