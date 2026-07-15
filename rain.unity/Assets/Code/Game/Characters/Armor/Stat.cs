using System;
using UnityEngine;

namespace Code.Game.Characters.Armor
{
    [Serializable]
    public class Stat
    {
        [field: SerializeField] public EStatType Type { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
    }
}