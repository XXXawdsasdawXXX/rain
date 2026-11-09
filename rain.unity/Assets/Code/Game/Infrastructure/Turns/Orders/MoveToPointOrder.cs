using Code.Game.Characters;
using Code.Game.Characters.Player.Commands;
using UnityEngine;

namespace Code.Game.World.Turns.Orders
{
    public class MoveToPointOrder : IActorOrder
    {
        private readonly CharacterActor _owner;
        private readonly Vector3Int _destination;


        public MoveToPointOrder(CharacterActor owner, Vector3Int destination)
        {
            _owner = owner;
            _destination = destination;
        }
        
        public void OnAssigned()
        {
            _owner.PathfindingFollower.SetPath(_owner.Model.Position, _destination);
        }

        public IActorCommand BuildCommand()
        {
            if (_owner.PathfindingFollower.HasActivePath)
            {
                return new PathStepCommand(_owner);
            }
                
            return new WaitCommand();
        }

        public bool IsComplete()
        {
            return !_owner.PathfindingFollower.HasActivePath;
        }

        public void Cancel()
        {
            _owner.PathfindingFollower.Clear();
        }
    }
}