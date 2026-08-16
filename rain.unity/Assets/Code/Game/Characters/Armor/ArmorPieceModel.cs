using System;
using TriInspector;
using UnityEngine;

namespace Code.Game.Characters.Armor
{
    [Serializable]
    public class ArmorPieceModel
    {
        [field: SerializeField, GUIColor(0.4f, 0.8f, 0.5f, 0.9f)] public EBodyPart Part { get; private set; }
        [field: SerializeField] public string ID { get; private set; } 
        [field: SerializeField, PreviewObject] public Sprite[] Views { get; private set; }
        [field: SerializeField] public Stat[] Stats { get; private set; }

        
        
        public ArmorPieceModel(EBodyPart part, Sprite[] views, Stat[] stats)
        {
            Part = part;
            Views = views;
            Stats = stats;
        }
        
        public void UpdateID(int setIndex)
        {
            ID = $"{setIndex}_{(int)Part}";
        }

        public bool TryGetStat(EStatType type, out Stat stat)
        {
            stat = null;
            
            foreach (Stat armorStat in Stats)
            {
                if (armorStat.Type == type)
                {
                    stat = armorStat;
                    return true;
                }
            }

            return false;
        }

        public string GetLocalizationKey()
        {
            return $"{Part}_{ID}";
        }
    }
}