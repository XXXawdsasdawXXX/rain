using System;
using System.Linq;
using Code.Game.Characters;
using Code.Game.Characters.Armor;
using Essential;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Components
{
    [Serializable]
    public class UIArmorPieceRender
    {
        [field: SerializeField]
        [field: GUIColor(0.4f, 0.8f, 0.5f, 0.9f)]
        public EBodyPart Part { get; private set; }

        [field: SerializeField] public Image[] Renderers { get; private set; }

        public UIArmorPieceRender(EBodyPart part, Image[] renderers)
        {
            Part = part;
            Renderers = renderers;
        }
    }


    [Serializable]
    public class UIArmorRender
    {
        /*
        Mask = 0,
        Head = 1,
        Torso = 2,
        Pants = 3,
        Shoes = 4,
        */

        [SerializeField] private UIArmorPieceRender[] _render =
        {
            new(EBodyPart.Mask, new Image[1]),
            new(EBodyPart.Head, new Image[1]),
            new(EBodyPart.Torso, new Image[1]) // 0 - main body | 1 - left hand | 2 - right hand
        };

        private CharacterWorldActor _actor;
        private ArmoryLibrary _armoryLibrary;


        public void SetArmorLibrary(ArmoryLibrary armoryLibrary)
        {
            _armoryLibrary = armoryLibrary;
        }

        public void SetHero(CharacterWorldActor actor)
        {
            DisposeHero();

            _actor = actor;
            _actor.Model.ArmorChanged += _updateView;

            foreach ((EBodyPart part, int setIndex) in _actor.Model.Armor)
            {
                _updateView(part, setIndex);
            }
        }

        public void DisposeHero()
        {
            if (_actor == null)
            {
                return;
            }
            _actor.Model.ArmorChanged -= _updateView;
            _actor = null;
        }

        public Sprite[] GetCurrentArmorSprites(EBodyPart bodyPart)
        {
            Image[] renderers = _render[(int)bodyPart].Renderers;
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

            foreach (ArmorPieceModel armorPieceModel in armorSetModel.Pieces) _setArmorPiece(armorPieceModel);
        }

        private void _updateView(EBodyPart part, int setIndex)
        {
            if (_armoryLibrary.TryGetPiece(setIndex, part, out ArmorPieceModel model))
            {
                _setArmorPiece(model);
            }
        }

        private void _setArmorPiece(ArmorPieceModel model)
        {
            int index = (int)model.Part;

            if (index >= _render.Length) return;

            UIArmorPieceRender pieceRender = _render[index];

            for (int i = 0; i < pieceRender.Renderers.Length; i++)
            {
                Image renderer = pieceRender.Renderers[i];

                if (renderer == null) break;

                renderer.sprite = model.Views != null && model.Views.Length > i
                    ? model.Views[i]
                    : null;

                renderer.color = renderer.sprite == null ? Color.clear : Color.white;
            }
        }
    }
}