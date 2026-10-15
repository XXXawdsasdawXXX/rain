using System;
using System.Collections.Generic;
using Code.Game.Characters;
using Code.Game.Characters.Armor;
using Essential.Data;
using UnityEngine;

namespace Code.UI.Components
{
    [Serializable]
    public class UICharacterStats
    {
        [Serializable]
        private class StatView
        {
            [field: SerializeField] public EStatType StatType { get; private set; }
            [field: SerializeField] public UIText TextRangeValue { get; private set; }
            [field: SerializeField] public UIImage ImageFill { get; private set; }

            private CharacterWorldActor _worldActor;
            private ReactiveProperty<float> _property;
            
            
            public void InitializeStat(CharacterWorldActor worldActor, ReactiveProperty<float> property)
            {
                DisposeStat();

                _worldActor = worldActor;
                
                _property = property;
                _property.SubscribeProperty(_updateView);
                
                _updateView(property.Value);
            }

            public void DisposeStat()
            {
                if (_property != null)
                {
                    _property.UnsubscribeProperty(_updateView);
                    _property = null;
                }

                if (_worldActor != null)
                {
                    _worldActor = null;
                }
            }
            
            private void _updateView(float value)
            {
                float maxValue = _worldActor.GetMaxStatsValue(StatType);
            
                if (ImageFill != null)
                {
                    ImageFill.SetFill(value / maxValue);
                }

                if (TextRangeValue != null)
                {
                    TextRangeValue.SetText($"{Mathf.RoundToInt(value)} / {Mathf.RoundToInt(maxValue)}");
                }
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------
    
        [SerializeField] private StatView[] _views;

        private List<StatView> _followers = new();

        private CharacterWorldActor _actor;

        
        public void SetHero(CharacterWorldActor actor)
        {
            DisposeHero();
            
            if (actor != null)
            {
                _actor = actor;

                foreach (StatView statView in _views)
                {
                    if (_actor.Model.Stats.TryGetValue(statView.StatType, out ReactiveProperty<float> property))
                    {
                        statView.InitializeStat(_actor, property);
                        _followers.Add(statView);
                    }
                }
            }
        }

        public void DisposeHero()
        {
            if (_actor == null)
            {
                return;
            }
            
            foreach (StatView view in _followers)
            {
                view.DisposeStat();
            }

            _followers.Clear();
                
            _actor = null;
        }
    }
}