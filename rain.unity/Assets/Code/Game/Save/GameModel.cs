using System;
using Essential;
using Essential.Data;
using UnityEngine.Serialization;

namespace Code.Game.Save
{
    [Serializable]
    public class GameModel
    {
        public string ID;
        public CharacterModel hero;
        public WorldModel World;
        public TimeSpan GameTime;
        public TimeSpan EnterTime;
        public TimeSpan ExitTime;
    }

    [Serializable]
    public class CharacterModel
    {
        public string Name;
        public int Health;
        public Position Position;

        public CharacterModel(string name)
        {
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