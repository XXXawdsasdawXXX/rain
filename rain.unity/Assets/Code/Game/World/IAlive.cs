using Essential.Data;

namespace Code.Game.World
{
    public interface IAlive
    {
        ReactiveProperty<float> Health { get; }
        void TakeDamage(float value);
        void Dead();
    }
}