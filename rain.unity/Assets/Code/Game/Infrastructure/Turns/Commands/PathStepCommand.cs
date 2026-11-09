using Code.Game.World.Turns;
using Essential;
using Essential.Extensions;
using UnityEngine;

namespace Code.Game.Characters.Player.Commands
{
    public class PathStepCommand : IActorCommand
    {
        private readonly CharacterActor _owner;
        
        public PathStepCommand(CharacterActor actor)
        {
            _owner = actor;
        }

        public void Execute()
        {
            if (_owner.PathfindingFollower.TryMoveToNextCell(out Vector3Int nextPosition))
            {
                _owner.SetCellPosition(nextPosition);
            }
        }
    }

    public class AttackTargetCommand : IActorCommand
    {
        private readonly CharacterActor _owner;
        private readonly CharacterActor _target;
        
        public AttackTargetCommand(CharacterActor owner, CharacterActor target)
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