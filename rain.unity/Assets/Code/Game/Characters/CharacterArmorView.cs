using System;
using System.Collections.Generic;
using System.Linq;
using Essential;
using TriInspector;
using UnityEngine;

namespace Code.Game.Characters
{
    [Serializable]
    public class CharacterArmorView
    {
        [Serializable]
        public class PieceView
        {
            [field: SerializeField] public EBodyPart Part { get; private set; }
            [field: SerializeField] public SpriteRenderer Renderer { get; private set; }
        }

        [Serializable]
        public class ArmorPieceView
        {
            [field: SerializeField] public EBodyPart MainPart { get; private set; }
            [field: SerializeField] public PieceView[] Pieces { get; private set; }
        }

        [SerializeField] private ArmorPieceView[] _armorPieceViews;
        [SerializeField] private ArmorPieceView[] _skinPieceViews;

        private Dictionary<EBodyPart, PieceView[]> _armorPieces = new();
        private Dictionary<EBodyPart, PieceView[]> _skinPieces = new();


        public void Initialize()
        {
            foreach (ArmorPieceView armorPieceView in _armorPieceViews)
            {
                if (_armorPieces.ContainsKey(armorPieceView.MainPart))
                {
                    Log.Error(this, $"[initialize] duplicate key {armorPieceView.MainPart}");
                    continue;
                }

                _armorPieces.Add(armorPieceView.MainPart, armorPieceView.Pieces);
            }

            foreach (ArmorPieceView armorPieceView in _skinPieceViews)
            {
                if (_skinPieces.ContainsKey(armorPieceView.MainPart))
                {
                    Log.Error(this, $"[initialize] duplicate key {armorPieceView.MainPart}");
                    continue;
                }

                _skinPieces.Add(armorPieceView.MainPart, armorPieceView.Pieces);
            }
        }

        [Button]
        public void SetArmorSet(ArmorSetModel armorSetModel)
        {
            if (_armorPieceViews.Length == 0)
            {
                Initialize();
            }
            
            foreach (ArmorPieceModel armor in armorSetModel.Pieces)
            {
                foreach (ArmorPieceModel.CompletePiecesModel piece in armor.CompleteViewPieces)
                {
                    _updatePieceView(armor.MainSlot, piece);
                }
            }
        }

        [Button]
        public void SetArmorPiece(ArmorSetModel armorSetModel, EBodyPart mainPart)
        {
            if (_armorPieceViews.Length == 0)
            {
                Initialize();
            }

            ArmorPieceModel data = armorSetModel.Pieces.FirstOrDefault(a => a.MainSlot == mainPart);

            if (data == null)
            {
                return;
            }
            
            foreach (ArmorPieceModel.CompletePiecesModel piece in data.CompleteViewPieces)
            {
                _updatePieceView(mainPart, piece);
            }
        }

        private void _updatePieceView(EBodyPart mainPart, ArmorPieceModel.CompletePiecesModel piece)
        {
            Dictionary<EBodyPart, PieceView[]> collection = piece.IsSkin ? _skinPieces : _armorPieces;

            if (!collection.ContainsKey(mainPart))
            {
                return;
            }

            PieceView armorView = collection[mainPart].FirstOrDefault(s => s.Part == piece.Slot);

            if (armorView != null)
            {
                armorView.Renderer.sprite = piece.ViewSprites;
                armorView.Renderer.transform.localPosition = piece.Offset;
                armorView.Renderer.transform.rotation = Quaternion.Euler(piece.RotationEuler);
            }
        }
    }
}