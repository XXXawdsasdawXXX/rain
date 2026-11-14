using System;
using Essential.Data;
using UnityEngine;

namespace Code.Game.World
{
    [Serializable]
    public sealed class EnvironmentHealth : IHealth
    {
        
        public ReactiveProperty<float> Health { get; } = new(0);
        public ReactiveProperty<bool> IsDead { get; } = new(false);

        [field: SerializeField] public bool IsAlive { get; private set; }
        [field: SerializeField] public float MaxHealth { get; private set; } = 1;

        
        public void ResetHealth()
        {
            Health.SetValueWithoutNotify(MaxHealth);
        }
        
        public void SetMaxHealth(float maxHealth)
        {
            MaxHealth = maxHealth;
        }
        
        public void TakeDamage(float value)
        {
            Health.Value -= value;
        }
    }
}