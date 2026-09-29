using Code.Game.Characters.Armor;
using Code.Game.Characters.Player.Commands;
using Code.Game.Save;
using Code.Game.World.Turns;
using Code.Pathfinding;

namespace Code.Game.Characters
{
    public class CharacterWorldActor : IWorldActor
    {
        public CharacterModel Model { get; }
        public WorldCharacterRender Render { get; } 
        public IPathfindingFollower PathfindingFollower { get; }
        
        private readonly ArmoryLibrary _armoryLibrary;
        
        private IActorCommand _command;
        private IActorOrder _order;
        
        
        public CharacterWorldActor(
            CharacterModel model,
            WorldCharacterRender render, 
            IPathfindingFollower pathfinding, 
            ArmoryLibrary armoryLibrary)
        {
            Model = model;
            PathfindingFollower = pathfinding;
         
            Render = render;
            Render.SetActor(this);
            
            _armoryLibrary = armoryLibrary;
        }
        
        public void OnWorldTurnBegin()
        {
            _command = _order?.BuildCommand() ?? new WaitCommand();
            _command.Execute();

            if (_order != null && _order.IsComplete())
            {
                _order.Cancel();
            }
        }

        public void SetOrder(IActorOrder order)
        {
            _order?.Cancel();
            _order = order;
            _order.OnAssigned();
        }

        public void SetCommand(IActorCommand command)
        {
            _command = command;
        }

        public void SetSelected(bool isSelected)
        {
            Render.SetStateForSelectedView(isSelected);
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