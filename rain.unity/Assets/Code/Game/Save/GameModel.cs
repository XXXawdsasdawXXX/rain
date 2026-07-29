using System;
using System.Collections.Generic;
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

        public CharacterModel(string name)
        {
            ID = Guid.NewGuid().ToString(); 
            Name = name;
            Health = 100;
            Position = new Position();
        }
    }

    [Serializable]
    public class WorldModel
    {
        
    }
}