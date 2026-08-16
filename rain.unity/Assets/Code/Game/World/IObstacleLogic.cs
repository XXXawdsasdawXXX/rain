using System;
using TriInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Game.World
{
    
    public enum EObstacleLogic
    {
        Static,
        Retractable, // выдвижные шипы
        Elemental, // можно вызвать реакцию на  другую стихией 
        Pulse // переодиченые волны урона
    }
    
    public interface IObstacleLogic
    {
        EObstacleLogic LogicType { get; }
        void Step();
    }

    public class RetractableObstacleLogic : IObstacleLogic
    {
        [Serializable]
        public class Model
        {
            [Title("Params")] 
            [field: SerializeField] public float BaseDamage { get; private set; }
            [field: SerializeField, Range(0, 100)] public float AttackChance { get; private set; }
            
            [Title("View")]
            [field: SerializeField] public Sprite SpriteIdle { get; private set; }
            [field: SerializeField] public Sprite[] SpritesAttackSequence { get; private set; }
        }

        public EObstacleLogic LogicType => EObstacleLogic.Retractable;

        private readonly Obstacle _owner;
        private readonly Model _model;


        public RetractableObstacleLogic(Obstacle owner, Model model)
        {
            _owner = owner;
            _model = model;
        }
        
        public void Step()
        {
            if (_owner.AliveInside.Count == 0)
            {
                return;
            }
            
            if (Random.Range(0, _model.AttackChance) < _model.AttackChance)
            {
                foreach (IAlive alive in _owner.AliveInside)
                {
                    alive.TakeDamage(_model.BaseDamage);
                }
            }
            

        }
    }
}