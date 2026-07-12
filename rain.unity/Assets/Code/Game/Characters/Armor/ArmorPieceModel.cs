using System;
using TriInspector;
using UnityEngine;

namespace Code.Game.Characters
{
    [Serializable]
    public class ArmorPieceModel
    {
        [Serializable]
        public class CompletePiecesModel
        {
            [field: SerializeField] public EBodyPart Slot { get; private set; }
            [field: SerializeField] public bool IsSkin { get; private set; }
            [field: SerializeField, PreviewObject] public Sprite ViewSprites { get; private set; }
            [field: SerializeField] public Vector3 Offset { get; private set; }
            [field: SerializeField] public Vector3 RotationEuler { get; private set; }
        }

        [field: SerializeField] public int ArmorSetID { get; private set; }
        [field: SerializeField] public EBodyPart MainSlot { get; private set; } // enum, указывающий часть тела
        [field: SerializeField] public Stat Stat { get; private set; }

        [field: SerializeField] public CompletePiecesModel[] CompleteViewPieces { get; private set; }

        public ArmorPieceModel(EBodyPart bodyPart)
        {
            MainSlot = bodyPart;
        }
        
        public ArmorPieceModel(EBodyPart bodyPart, int armorSetID)
        {
            MainSlot = bodyPart;
            ArmorSetID = armorSetID;
        }

        public void SetId(int armorSetID)
        {
            ArmorSetID = armorSetID;
        }
    }
}