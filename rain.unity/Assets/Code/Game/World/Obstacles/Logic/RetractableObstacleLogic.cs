using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Game.World
{
    [Serializable]
    public class RetractableObstacleLogic : AObstacleLogic
    {
        [Serializable]
        public class Model
        {
            [field: SerializeField] public float BaseDamage { get; private set; }
            [field: SerializeField, Range(0, 100)] public float AttackChance { get; private set; }
        }
        
        [SerializeField] private Model _model;

        
        public override void Step()
        {
            if (owner == null || owner.GetAlive().Count == 0)
            {
                return;
            }

            float chance = Random.Range(0f, 100f); 
            
            if (chance < _model.AttackChance)
            {
                foreach (IHealth alive in owner.GetAlive())
                {
                    alive.TakeDamage(_model.BaseDamage);
                }
            }
        }
    }
}