using System;
using Essential;
using TriInspector;
using UnityEngine;

namespace Code.Game.Characters.Armor
{
    [Serializable]
    public class ArmorPieceModel
    {

        [field: SerializeField, GUIColor(0.4f, 0.8f, 0.5f, 0.9f)] public EBodyPart Part { get; private set; }
        [field: SerializeField] public string ID { get; private set; } 
        [field: SerializeField] public Sprite[] Views { get; private set; }

        [field: SerializeField] public Stat Stat { get; private set; }

        public ArmorPieceModel(EBodyPart part, Sprite[] views)
        {
            Part = part;
            Views = views;
        }


        public void UpdateID(int setIndex)
        {
            ID = $"{setIndex}_{(int)Part}";
        }                                                                                                                                                                                            

        public string GetLocalizationKey()
        {
            return $"{Part}_{ID}";
        }
    }
}