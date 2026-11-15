using System;
using UnityEngine;
using VContainer;

namespace Code.Game.World.Characters
{
    public class CharacterWorldRender : WorldMono, IDisposable
    {
        [field: SerializeField] public GameObject SelectView { get; set; }
        [field: SerializeField] public WorldArmorRender WorldArmor { get; private set; }

        private ArmoryLibrary _armoryLibrary;

        
        [Inject]
        public void Construct(ArmoryLibrary library)
        {
            _armoryLibrary = library;
         
            WorldArmor.SetArmorLibrary(_armoryLibrary);
        }
        
        public void Dispose()
        {
            WorldArmor.DisposeHero();
        }
        
        public void SetActor(CharacterActor actor)
        {
            if (actor != null)
            {
                Dispose();
            }

            WorldArmor.SetHero(actor);
        }
        
        public void SetStateForSelectedView(bool isSelect)
        {
            SelectView.SetActive(isSelect);
        }
    }
}