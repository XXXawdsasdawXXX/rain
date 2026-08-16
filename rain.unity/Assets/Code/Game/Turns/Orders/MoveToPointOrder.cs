using Code.Game.Characters;
using Code.Game.Characters.Player.Commands;
using UnityEngine;

namespace Code.Game.World.Turns.Orders
{
    public class MoveToPointOrder : IActorOrder
    {
        private readonly CharacterWorldActor _owner;
        private readonly Vector3 _destination;


        public MoveToPointOrder(CharacterWorldActor owner, Vector3 destination)
        {
            _owner = owner;
            _destination = destination;
        }
        
        public void OnAssigned()
        {
            _owner.PathfindingFollower.SetPath(_owner.Render.Body.position, _destination);
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