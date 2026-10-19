namespace Code.Game.World.Turns
{
    public interface IWorldTurnBeginListener
    {
        public enum ETurnPriority
        {
            Character,
            Obstacle,
            Other
        }
        
        public ETurnPriority Priority { get; }
        void OnWorldTurnBegin();
    }
}