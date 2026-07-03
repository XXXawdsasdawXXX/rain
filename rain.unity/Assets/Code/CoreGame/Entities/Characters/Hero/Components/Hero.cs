using System;
using System.Collections.Generic;
using Code.CoreGame.Entities.Animation;
using Code.CoreGame.Entities.Characters.Controllers;
using Code.CoreGame.Entities.Characters.Hero.Data;
using Code.CoreGame.Entities.Select;
using Core.GameLoop;
using Core.Input;
using Core.Save.Models;
using Core.ServiceLocator;
using Essential;
using UnityEngine;

namespace Code.CoreGame.Entities.Characters.Hero.Components
{
    public class Hero : Character, ISubscriber
    {
        public HeroModel Model { get; private set; }
        
        [field: Header("Unity components")]
        [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }
        
        [field: Space]
        
        [field: Header("custom components")]
        [field: SerializeField] public CurrentMaxValue CurrentMaxValue { get; private set; }
        [field: SerializeField] public HeroColor Color { get; private set; }
        [field: SerializeField] public HeroAnimation Animation { get; private set; }
        [field: SerializeField] public SelectObject SelectObject { get; private set; }

        /*
        public override void OnStartClient()
        {
            base.OnStartClient();
            
            if (!IsConstructed)
            {
                if (IsOwner)
                {
                    InitializeComponents();
                }
                else
                {
                    Destroy(Rigidbody);
                }
                
                IsConstructed = true;
            }

            Container.Instance.GetService<HeroSpawner>().RegisterHeroListeners(this);
        }
        */
        
        
        public override void InitializeComponents()
        {
            
            InputService inputService = Container.Instance.GetService<InputService>();
            HeroSettings heroSettings = Container.Instance.GetSO<HeroSettings>();
            GameModel gameModel = Container.Instance.GetService<GameModel>();
            
            Model = gameModel.Hero;
            
            Movement movement = new(Rigidbody, inputService.Direction, heroSettings.MoveSpeed);
            Components.Add(typeof(Movement), movement);
            
            Mainer mainer = new(Animation, CurrentMaxValue);
            Components.Add(typeof(Mainer), mainer);

            HeroSelectController selectController = new(this);
            Components.Add(typeof(HeroSelectController), selectController);
            
            movement.Condition.Add(() => Animation.CurrentState is not 
                AnimatorKey.ECharacterAnimationState.Eat and not 
                AnimatorKey.ECharacterAnimationState.Harvest);
            
            mainer.Condition.Add(() => inputService.Direction.Value == Vector2.zero);

            foreach (KeyValuePair<Type, ICharacterComponent> characterComponent in Components)
            {
                characterComponent.Value.Condition.Add(() => CurrentMaxValue.Current > 0);
                characterComponent.Value.Condition.Add(() => !Model.InBattle);
            }
            
            Log.Info(this, "init");

            CurrentMaxValue.SetCurrentValue(Model.Stats.Health);
        }

        public void Subscribe()
        {
            CurrentMaxValue.Changed += _onHealthChanged;
        }

        public void Unsubscribe()
        {   
            CurrentMaxValue.Changed -= _onHealthChanged;
        }

        private void _onHealthChanged(int current)
        {
            Model.Stats.Health = current;
        }
    }
}