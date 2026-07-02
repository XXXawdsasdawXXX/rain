using System;
using System.Collections.Generic;

namespace Core.Save
{
    public class CharacterModel
    {
        
    }
    [Serializable]
    public class HeroModel
    {
        public string HeroId;
        
        public string Name;
        public int Health;
        public int Armor;
        public bool InBattle;

        public CharacterStats Stats;
    
        public Dictionary<int, int> ResourcesStorage = new();

        public List<string> CardCollection; // все карты которые есть у игрока (id)
        public List<string> Deck;           // собранная колода (id), макс 30
        public string SelectedDeckId;
        
        public TimeSpan CreateTime;
        public TimeSpan GameTime;
        public TimeSpan ExitTime;
        
        
        public HeroModel()
        {
            Name = "name";
            Health = 100;

            ResourcesStorage = new Dictionary<int, int>()
            {
                { 1, 100 }, // gold
                { 2, 0 }, // starwberry
                { 3, 0 }, // cristal
            };

            GameTime = new TimeSpan();
            ExitTime = new TimeSpan();

            Stats = new CharacterStats();

            CardCollection = new List<string>();
            Deck = new List<string>();
            SelectedDeckId = string.Empty;
        }
    }
    
    [Serializable]
    public class CharacterStats
    {
        public int Agility;    
        public int Strength;   
        public int Endurance;  
        public int Intellect;  
    }
}