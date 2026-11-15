namespace Code.Game.Infrastructure.Turns
{
    public interface IActorOrder
    {
        void OnAssigned();
        IActorCommand BuildCommand();
        bool IsComplete();
        void Cancel();
    }
}