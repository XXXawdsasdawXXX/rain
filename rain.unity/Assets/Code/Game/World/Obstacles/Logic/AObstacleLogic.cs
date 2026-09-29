using System;
using TriInspector;

namespace Code.Game.World
{
    [Serializable]
    public abstract class AObstacleLogic
    {
        public EObstacleState State { get; }
        protected Obstacle owner;

        public virtual void Bind(Obstacle owner)
        {
            this.owner = owner;
        }
        
        public abstract void Step();
        public virtual void Dispose(){}
    }
}
