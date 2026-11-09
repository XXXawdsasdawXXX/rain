using System;
using System.Collections.Generic;
using System.Linq;
using Code.Game.Characters.Armor;
using Code.Game.World;
using Essential;
using Essential.Data;
using UnityEngine;

namespace Code.Game.Save
{
    [Serializable]
    public class GameModel
    {
        public string ID { get; private set; }

        public Dictionary<EEntityType, List<CharacterModel>> Characters { get; private set; }
        public WorldModel World { get; private set; }
        
        public TimeSpan GameTime { get; private set; }
        public TimeSpan EnterTime { get; private set; }
        public TimeSpan ExitTime { get; private set; }

        public GameModel(WorldModel world, CharacterModel hero)
        {
            ID = Guid.NewGuid().ToString();
            World = world;
            Characters = new Dictionary<EEntityType, List<CharacterModel>>()
            {
                { EEntityType.Hero, new List<CharacterModel>() { hero } },
                { EEntityType.Npc, new List<CharacterModel>() },
                { EEntityType.Enemy, new List<CharacterModel>() }
            };
            
            EnterTime = DateTime.UtcNow.TimeOfDay;
            ExitTime = DateTime.UtcNow.TimeOfDay;
            GameTime = TimeSpan.Zero;
        }

        public void UpdateEnterTime()
        {
            EnterTime = DateTime.UtcNow.TimeOfDay;
        }

        public void UpdateExitTime()
        {
            ExitTime = DateTime.UtcNow.TimeOfDay;
            GameTime += ExitTime - EnterTime;
        }
    }

    [Serializable]
    public class CharacterModel
    {
        public event Action<EBodyPart, int> ArmorChanged; 

        public string ID { get; private set; }
        public string Name { get; private set; }
        public Vector3Int Position { get; private set; }
        public Dictionary<EBodyPart, int> Armor { get; private set; }
        public Dictionary<EStatType, ReactiveProperty<float>> Stats { get; private set; }
        public Dictionary<EStatType, float> DefaultMaxStats { get; private set; }

        
        public CharacterModel(string name)
        {
            ID = Guid.NewGuid().ToString(); 
            Name = name;
            Position = Vector3Int.zero;
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

        public void SetCellPosition(Vector3Int cellPosition)
        {
            Position = cellPosition;
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