using System.Collections.Generic;
using Code.Game.World.Characters;
using Essential.Data;
using Essential.Data.BoolMatrix;
using UnityEngine;

namespace Code.Game.Ability
{
    public static class AbilityResolver
    {
        public static IReadOnlyList<Vector3Int> GetCells(BoolMatrix zone, Vector3Int origin)
        {
            Vector3Int zoneOrigin = zone.GetCenteredMatrixOriginCell();
            
            List<Vector2Int> indexes = new();
            List<Vector3Int> worldCells = new();
            
            zone.GetEnableIndexes(indexes);
            
            foreach (Vector2Int index in indexes)
            {
                Vector3Int worldCell = origin + zoneOrigin + new Vector3Int(index.x, index.y);
                worldCells.Add(worldCell);
            }

            return worldCells;
        }

        public static bool CanUse(CharacterActor owner, AbilityModel ability)
        {
            if (owner.Model.Stats.TryGetValue(EStatType.Energy, out ReactiveProperty<float> ownerEnergy))
            {
                if (ownerEnergy.Value >= ability.EnergyPrice)
                {
                    return true;
                }
            }

            return false;
        }

        public static float GetDamage(this AbilityModel model)
        {
            float damage = 0;
            
            foreach (Stat stat in model.Stats)
            {
                switch (stat.Type)
                {
                    case EStatType.None:
                    case EStatType.Health:
                    case EStatType.Noise:
                    case EStatType.Energy:
                    default:
                        break;
                    case EStatType.Damage:
                        damage += stat.Value;
                        break;
                            
                    case EStatType.CriticalChance:
                        if (Random.Range(0, 100) < stat.Value)
                        {
                            damage *= 1.2f;
                        }
                        break;
                }
            }

            return damage;
        }
    }
    
    
}