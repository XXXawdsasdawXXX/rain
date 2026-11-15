using System;

namespace Code.Game.World.Obstacles
{
    [Serializable]
    public class ExplosionObstacleLogic : AObstacleLogic
    {
        public override EObstacleState Step()
        {
            return EObstacleState.Idle;
        }
    }
}