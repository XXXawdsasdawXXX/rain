using Code.Game.World.Turns;
using Essential;
using UnityEngine;

namespace Code.Game.Characters.Player.Commands
{
    public class PathStepCommand : IActorCommand
    {
        private readonly CharacterWorldActor _owner;
        
        public PathStepCommand(CharacterWorldActor actor)
        {
            _owner = actor;
        }

        public void Execute()
        {
            if (_owner.PathfindingFollower.TryMoveToNextWorldPosition(out Vector3 nextPosition))
            {
                _owner.View.Body.position = nextPosition;
                _owner.Model.Position = new Position(nextPosition.x, nextPosition.y);
            }
        }
    }

    public class AttackTargetCommand : IActorCommand
    {
        private readonly CharacterWorldActor _owner;
        private readonly CharacterWorldActor _target;

        
        public AttackTargetCommand(CharacterWorldActor owner, CharacterWorldActor target)
        {
            _owner = owner;
            _target = target;
        }
        
        public void Execute()
        {
            //todo take damage
            //todo play damage animation
            //todo check to death
            
            Log.Info(this, "[execute] test take damage");
        }
    }
}