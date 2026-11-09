using Code.Game.Characters.Armor;
using Code.Game.Characters.Player.Commands;
using Code.Game.Save;
using Code.Game.World;
using Code.Game.World.Turns;
using Code.Pathfinding;
using Essential.Data;
using Unity.Mathematics;
using UnityEngine;

namespace Code.Game.Characters
{
    public class CharacterActor : IWorldActor, IHealth
    {
        public IWorldTurnBeginListener.ETurnPriority Priority => IWorldTurnBeginListener.ETurnPriority.Character;
        public ReactiveProperty<float> Health => Model.Stats[EStatType.Health];
        public ReactiveProperty<bool> IsDead { get; } = new(false);
        public CharacterModel Model { get; }
        public CharacterWorldView View { get; } 
        public IPathfindingFollower PathfindingFollower { get; }

        private readonly ITilemapView _tilemap;
        private readonly ArmoryLibrary _armoryLibrary;
        
        private IActorCommand _command;
        private IActorOrder _order;
        
        
        public CharacterActor(
            CharacterModel model,
            CharacterWorldView view, 
            IPathfindingFollower pathfinding, 
            ITilemapView tilemap,
            ArmoryLibrary armoryLibrary)
        {
            Model = model;
            PathfindingFollower = pathfinding;
            View = view;

            _tilemap = tilemap;
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

        public void SetCellPosition(Vector3Int position)
        {
            Model.SetCellPosition(position);
            View.Body.position = _tilemap.CellAsCenterWorld(position);
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
            View.Render.SetStateForSelectedView(isSelected);
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
        
        public void TakeDamage(float value)
        {
            if (IsDead.Value)
            {
                return;
            }
            
            Health.Value = math.max(0, Health.Value - value);

            if (Health.Value == 0)
            {
                IsDead.Value = true;
            }
        }
    }
}