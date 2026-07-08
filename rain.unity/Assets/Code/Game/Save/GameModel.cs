using System;
using Essential.Data;

namespace Code.Game.Save
{
    [Serializable]
    public class GameModel
    {
        public string ID;
        public HeroModel Hero;
        public WorldModel World;
        public TimeSpan GameTime;
        public TimeSpan EnterTime;
        public TimeSpan ExitTime;
    }

    [Serializable]
    public class HeroModel
    {
        public string Name;
        public int Health;

        public HeroModel(string name)
        {
            Name = name;
            Health = 100;
        }
    }

    [Serializable]
    public class WorldModel
    {
        
    }
}