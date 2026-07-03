using System;

namespace Code.CoreGame.Entities.Characters.Hero.Components
{
    [Serializable]
    public class CurrentMaxValue
    {
        public event Action<int> Changed;
        public event Action Decreased;
        
        public int Current { get; set; }
        
        
        public void SetCurrentValue(int value)
        {
            Current = value;
            Changed?.Invoke(Current);
        }

        public void Decrease(int value)
        {
            Current -= value;
            Decreased?.Invoke();
            Changed?.Invoke(Current);
        }
    }
}