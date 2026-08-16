using System.Linq;
using Code.Game.Characters.Armor;
using Code.Game.Characters.Player.Commands;
using Code.Game.Save;
using Code.Game.World.Turns;
using Code.Pathfinding;

namespace Code.Game.Characters
{
    public class CharacterWorldActor : IWorldActor
    {
        private readonly ArmoryLibrary _armoryLibrary;
        public string ID => View.Model != null ? View.Model.ID : string.Empty;
        public CharacterModel Model => View.Model;
        public CharacterView View { get; } 
        public IPathfindingFollower PathfindingFollower { get; }
        public IActorCommand Command { get; private set; }
        public IActorOrder Order { get; private set; }
        
        
        public CharacterWorldActor(CharacterView view, IPathfindingFollower pathfinding, ArmoryLibrary armoryLibrary)
        {
            PathfindingFollower = pathfinding;
            View = view;
            _armoryLibrary = armoryLibrary;
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
        
        
        public float GetMaxStatsValue(EStatType stat)
        {
            float maxValue = 0;

            if (Model.DefaultMaxStats.TryGetValue(stat, out float defaultValue))
            {
                maxValue += defaultValue;
            }
            
            foreach ((EBodyPart part, int armorIndex) in Model.Armor)
            {
                ArmorSetModel set = _armoryLibrary.Armory.Get(armorIndex);

                if (set.TryGetPiece(part, out ArmorPieceModel pieceModel) 
                    && pieceModel.TryGetStat(stat, out Stat armorStat))
                {
                    maxValue += armorStat.Value;
                }
            }

            return maxValue;
        }
    }
}