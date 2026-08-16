namespace Code.Game.World.Turns
{
    public interface IWorldActor : IWorldTurnBeginListener
    {
        void SetOrder(IActorOrder order);
    }
}