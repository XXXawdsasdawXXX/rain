using System;
using Code.Game.World.Turns;

namespace Code.Game.Characters.Player.Commands
{
    public class WaitCommand : IActorCommand
    {
        public event Action<IActorCommand> Completed;

        public void Execute()
        {
            
        }
    }
}