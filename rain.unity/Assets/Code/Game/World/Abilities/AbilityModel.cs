using System;
using System.Collections.Generic;
using Code.Game.World.Characters;
using Essential.Data.BoolMatrix;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Code.Game.World.Abilities
{
    public enum EAbilityType
    {
        None,
        Damage,
        Heal,
        Harvest,
        ApplyEffect
    }
    
    public enum ETargetingMode
    {
        None,
        FromCaster,
        AtCursor
    }

    [CreateAssetMenu(fileName = "Model_Ability", menuName = "Game/Model/Ability")]
    public class AbilityModel : ScriptableObject
    {
        [field: SerializeField] public int Index { get; private set; }
        [field: SerializeField] public EAbilityType Type { get; private set; }
        [field: SerializeField] public ETargetingMode TargetMode { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public Stat[] Stats { get; private set; }
        [field: SerializeField] public BoolMatrix Zone { get; private set; }



#if UNITY_EDITOR
        public void e_setIndex(int index)
        {
            Index = index;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}