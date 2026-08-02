using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Code.Game.Characters.Armor;
using Essential;
using Newtonsoft.Json;

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
        public int Health;
        public Position Position;
        public Dictionary<EBodyPart, int> Armor;

        
        public CharacterModel(string name)
        {
            ID = Guid.NewGuid().ToString(); 
            Name = name;
            Health = 100;
            Position = new Position();
            Armor = new Dictionary<EBodyPart, int>();

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