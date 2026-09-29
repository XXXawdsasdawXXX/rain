using Essential.Data;

namespace Code.Game.World
{
    public interface IHealth
    {
        ReactiveProperty<float> Current { get; }
        ReactiveProperty<bool> IsDead { get; }
        void TakeDamage(float value);
    }

    public class Health : IHealth
    {
        public ReactiveProperty<float> Current { get; } = new(0);
        public ReactiveProperty<bool> IsDead { get; } = new(false);

        
        public void TakeDamage(float value)
        {
            Current.Value -= value;

            if (Current.Value <= 0 && !IsDead.Value)
            {
                _die();
            }
        }

        private void _die()
        {
            if (Current.Value < 0)
            {
                Current.SetValueWithoutNotify(0);
            }

            IsDead.Value = false;
        }
    }
}