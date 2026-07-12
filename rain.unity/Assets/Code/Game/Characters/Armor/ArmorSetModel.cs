using TriInspector;
using UnityEngine;

namespace Code.Game.Characters
{
    [CreateAssetMenu(fileName = "Model_ArmorSet", menuName = "Game/Model/ArmorSet")]
    public class ArmorSetModel : ScriptableObject
    {
        [field: SerializeField] public int ID { get; private set; }

        [field:  SerializeField] public ArmorPieceModel[] Pieces =
        {
            new(EBodyPart.Head),
            new(EBodyPart.Torso),
            new(EBodyPart.LegLeft),
            new(EBodyPart.LegRight),
            new(EBodyPart.HandLeft),
            new(EBodyPart.HandRight),
            new(EBodyPart.ShoulderLeft),
            new(EBodyPart.ShoulderRight),
            new(EBodyPart.PantsLeft),
            new(EBodyPart.PantsRight),
        };

        public ArmorPieceModel GetPiece(EBodyPart bodyPart)
        {
            foreach (ArmorPieceModel piece in Pieces)
            {
                if (piece.MainSlot == bodyPart)
                {
                    return piece;
                }
            }

            Debug.LogError($"Armor has not piece of {bodyPart}");

            return null;
        }

#if UNITY_EDITOR
        [Button]
        public void UpdatePiecesSetID()
        {
            foreach (ArmorPieceModel armorPieceModel in Pieces)
            {
                armorPieceModel.SetId(ID);
            }
        }
#endif
    }
}