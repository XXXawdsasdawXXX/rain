using Essential.Data;

namespace Code.Game.World
{
    public interface IHealth
    {
        ReactiveProperty<float> Health { get; }
        ReactiveProperty<bool> IsDead { get; }
        void TakeDamage(float value);
    }
    
}