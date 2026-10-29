namespace Code.Game.World.Turns
{
    public interface IActorOrder
    {
        void OnAssigned();
        IActorCommand BuildCommand();
        bool IsComplete();
        void Cancel();
    }
}