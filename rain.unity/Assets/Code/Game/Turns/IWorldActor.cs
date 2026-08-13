namespace Code.Game.World.Turns
{
    public interface IWorldActor
    {
        void SetOrder(IActorOrder order);
        void SetCommand(IActorCommand command);
        void OnWorldTurnBegin();
    }
}