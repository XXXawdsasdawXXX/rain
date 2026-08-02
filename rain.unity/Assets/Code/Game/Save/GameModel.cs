using System;
using System.Collections.Generic;
using System.Linq;
using Code.Game.Characters.Armor;
using Essential;

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
        public string ID;
        public string Name;
        public int Health;
        public Position Position;
        public Dictionary<EBodyPart, string> Armor;

        public CharacterModel(string name)
        {
            ID = Guid.NewGuid().ToString(); 
            Name = name;
            Health = 100;
            Position = new Position();
            Armor = new Dictionary<EBodyPart, string>();

            EBodyPart[] bodyParts = Enum.GetValues(typeof(EBodyPart)).Cast<EBodyPart>().ToArray();
            
            foreach (EBodyPart t in bodyParts)
            {
                Armor.Add(t, string.Empty);
            }
        }
    }

    [Serializable]
    public class WorldModel
    {
        
    }
}