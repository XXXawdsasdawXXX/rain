using System;
using System.Collections.Generic;

namespace Core.Save.Models
{
    [Serializable]
    public class HeroModel : SavedModel
    {
        public string HeroId;
        
        public string Name;

        public bool InBattle;

        public CharacterStats Stats;
    
        public Dictionary<int, int> ResourcesStorage = new();

        public TimeSpan CreateTime;
        public TimeSpan GameTime;
        public TimeSpan ExitTime;
        
        
        public HeroModel()
        {
            Name = "name";
            Stats = new CharacterStats();
            ResourcesStorage = new Dictionary<int, int>()
            {
                { 1, 100 }, // gold
                { 2, 0 }, // starwberry
                { 3, 0 }, // cristal
            };

            GameTime = new TimeSpan();
            ExitTime = DateTime.UtcNow.TimeOfDay;
            CreateTime = DateTime.UtcNow.TimeOfDay;
        }
    }
    
    [Serializable]
    public class CharacterStats
    {
        public int Health;
        public int Armor;
        public int Agility;    
        public int Strength;   
        public int Endurance;  
        public int Intellect;  
    }
}