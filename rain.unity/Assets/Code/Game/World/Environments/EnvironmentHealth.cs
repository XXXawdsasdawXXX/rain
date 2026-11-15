using System;
using Essential;
using Essential.Data;
using UnityEngine;

namespace Code.Game.World.Environments
{
    [Serializable]
    public sealed class EnvironmentHealth : IHealth
    {
        public ReactiveProperty<float> Current { get; } = new(0);
        public ReactiveProperty<bool> IsDead { get; } = new(false);

        [field: SerializeField] public bool IsAlive { get; private set; }
        [field: SerializeField] public float MaxHealth { get; private set; } = 1;

        
        public void ResetHealth()
        {
            Current.SetValueWithoutNotify(MaxHealth);
            IsDead.Value = false;
        }
        
        public void SetMaxHealth(float maxHealth)
        {
            MaxHealth = maxHealth;
        }
        
        public void TakeDamage(float value)
        {
            if (IsDead.Value || !IsAlive)
            {
                return;
            }
            
            Current.Value = Mathf.Max(0, Current.Value - value);

            if (Current.Value == 0)
            {
                IsDead.Value = true;
            }
           
            Log.Info(this, $"current health = {Current.Value}/{MaxHealth}");
        }

        public void Heal(float heal)
        {
            if (IsDead.Value || !IsAlive)
            {
                return;
            }
            Current.Value = Mathf.Max(MaxHealth, Current.Value + heal);

            
        }
    }
}