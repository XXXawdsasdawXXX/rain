namespace Code.Game.World
{
    public interface IWorldEntityView
    {
        public string ID { get; }
        public EEntityType Type { get; }

        public bool TryGetHealth(out IHealth health);
    }
}