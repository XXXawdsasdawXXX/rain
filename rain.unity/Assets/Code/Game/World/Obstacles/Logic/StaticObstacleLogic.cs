using System;
using UnityEngine;

namespace Code.Game.World
{
    [Serializable]
    public class StaticObstacleLogic : AObstacleLogic
    {
        [Serializable]
        public class Model
        {
            [field: SerializeField] public float BaseDamage { get; private set; } 
        }

        [SerializeField] private Model _model;
        
        
        public override void Step()
        {
            if (owner == null || owner.GetAlive().Count == 0)
            {
                return;
            }

            foreach (IAlive alive in owner.GetAlive())
            {
                alive.TakeDamage(_model.BaseDamage);
            }
        }
    }
}