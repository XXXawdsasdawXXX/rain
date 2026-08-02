using System;
using Core.AssetManagement.Libraries;
using UnityEngine;

namespace Code.Game.Characters.Armor
{
    [CreateAssetMenu(fileName = "Library_Armory", menuName = "Game/Library/Armory")]
    public class ArmoryLibrary : ScriptableObject
    {
        [Serializable]
        public class SetLibrary : Library<int, ArmorSetModel>
        {
            protected override bool ThisIs(ArmorSetModel value, int key)
            {
                return value.SetIndex == key;
            }
        }

        [field: SerializeField] public SetLibrary Armory { get; private set; }
    }
}