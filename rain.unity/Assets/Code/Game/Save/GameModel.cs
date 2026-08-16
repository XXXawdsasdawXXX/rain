using System;
using System.Collections.Generic;
using System.Linq;
using Code.Game.Characters.Armor;
using Essential;
using Essential.Data;

namespace Code.Game.Save
{
    [Serializable]
    public class GameModel
    {
        public string ID;
        public List<CharacterModel> Heroes;
        public List<CharacterModel> Npc;
        public WorldModel World;
        public TimeSpan GameTime;
        public TimeSpan EnterTime;
        public TimeSpan ExitTime;
    }

    [Serializable]
    public class CharacterModel
    {
        public event Action<EBodyPart, int> ArmorChanged; 

        public string ID;
        public string Name;
        public Position Position;
        public Dictionary<EBodyPart, int> Armor;
        public Dictionary<EStatType, ReactiveProperty<float>> Stats;
        public Dictionary<EStatType, float> DefaultMaxStats;

        
        public CharacterModel(string name)
        {
            ID = Guid.NewGuid().ToString(); 
            Name = name;
            Position = new Position();
            Armor = new Dictionary<EBodyPart, int>();
            Stats = new Dictionary<EStatType, ReactiveProperty<float>>()
            {
                { EStatType.Health , new ReactiveProperty<float>(100)},
                { EStatType.Noise , new ReactiveProperty<float>(0)},
                { EStatType.Damage , new ReactiveProperty<float>(10)}
            };
            DefaultMaxStats = new Dictionary<EStatType, float>()
            {
                { EStatType.Health , 100},
                { EStatType.Noise , 100},
                
            };

            EBodyPart[] bodyParts = Enum.GetValues(typeof(EBodyPart)).Cast<EBodyPart>().ToArray();
            
            foreach (EBodyPart t in bodyParts)
            {
                Armor.Add(t, 0);
            }
        }

        public void SetArmor(EBodyPart bodyPart, int armorSetID)
        {
            if (!Armor.ContainsKey(bodyPart))
            {
                Log.Error(this, $"[SetArmor] has not part {bodyPart}");
                
                return;
            }

            Armor[bodyPart] = armorSetID;
            
            ArmorChanged?.Invoke(bodyPart, armorSetID);
        }
    }

    [Serializable]
    public class WorldModel
    {
        
    }
}