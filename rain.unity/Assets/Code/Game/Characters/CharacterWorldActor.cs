using Code.Game.Characters.Player.Commands;
using Code.Game.Save;
using Code.Game.World.Turns;
using Code.Pathfinding;

namespace Code.Game.Characters
{
    public class CharacterWorldActor : IWorldActor
    {
        public string ID => View.Model != null ? View.Model.ID : string.Empty;
        public CharacterModel Model => View.Model;
        public CharacterView View { get; } 
        public IPathfindingFollower PathfindingFollower { get; }
        public IActorCommand Command { get; private set; }
        public IActorOrder Order { get; private set; }
        
        
        public CharacterWorldActor(CharacterView view, IPathfindingFollower pathfinding)
        {
            PathfindingFollower = pathfinding;
            View = view;
        }
        
        public void OnWorldTurnBegin()
        {
            Command = Order?.BuildCommand() ?? new WaitCommand();

            Command.Execute();

            if (Order != null && Order.IsComplete())
            {
                Order.Cancel();
            }
        }

        public void SetOrder(IActorOrder order)
        {
            Order?.Cancel();
            Order = order;
            Order.OnAssigned();
        }

        public void SetCommand(IActorCommand command)
        {
            Command = command;
        }

        public void SetSelected(bool isSelected)
        {
            View.SetStateForSelectedView(isSelected);
        }
    }
}