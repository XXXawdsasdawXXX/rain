using System;
using Core.AssetManagement.Libraries;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace Code.Game.Ability
{
    [CreateAssetMenu(fileName = "Library_Ability", menuName = "Game/Library/Ability")]
    public class AbilityLibrary : ScriptableObject
    {
        [Serializable]
        public class Abilities : Library<int, AbilityModel>
        {
            protected override bool ThisIs(AbilityModel value, int key)
            {
                return value != null && value.Index == key;
            }
        }

        [field: SerializeField] public Abilities Library { get; private set; }

#if UNITY_EDITOR
        [Button]
        private void e_updateIndexes()
        {
            AbilityModel[] all = Library.GetAll();
         
            for (int i = 0; i < all.Length; i++)
            {
                AbilityModel model = all[i];
                model.e_setIndex(i);
            }

            EditorUtility.SetDirty(this);
        }
#endif
    }
}