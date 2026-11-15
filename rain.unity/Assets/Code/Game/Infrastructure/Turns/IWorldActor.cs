namespace Code.Game.Infrastructure.Turns
{
    public interface IWorldActor : IWorldTurnBeginListener
    {
        void SetOrder(IActorOrder order);
    }
}