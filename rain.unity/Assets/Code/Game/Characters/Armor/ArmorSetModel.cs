using System.Linq;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace Code.Game.Characters.Armor
{
    [CreateAssetMenu(fileName = "Model_ArmorSet", menuName = "Game/Model/ArmorSet")]
    public class ArmorSetModel : ScriptableObject
    {
        [field: SerializeField] public int SetIndex { get; private set; }

        [field: SerializeField] public ArmorPieceModel[] Pieces { get; private set; } = 
        {
            new(EBodyPart.Mask,  new Sprite[1], new Stat[]{}),
            new(EBodyPart.Head,  new Sprite[1], new Stat[]{}),
            new(EBodyPart.Torso, new Sprite[3], new Stat[]{}),// 0 - main body | 1 - left hand | 2 - right hand
            new(EBodyPart.Pants, new Sprite[2], new Stat[]{}),// 0 - left | 1 - right
            new(EBodyPart.Shoes, new Sprite[2], new Stat[]{}),// 0 - left | 1 - right
        };


        public bool TryGetPiece(EBodyPart part, out ArmorPieceModel pieceModel)
        {
            pieceModel = Pieces.FirstOrDefault(p => p.Part.Equals(part));
            return pieceModel != null;
        }
        
#if UNITY_EDITOR

        [Button]
        private void _updatePiecesID()
        {
            foreach (ArmorPieceModel pieceModel in Pieces)
            {
                pieceModel.UpdateID(SetIndex);
            }
            
            EditorUtility.SetDirty(this);
        }
#endif
    }
}