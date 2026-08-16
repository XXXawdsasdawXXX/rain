using System;
using Code.Game.Characters.Armor;
using Code.Pathfinding;
using TriInspector;
using UnityEngine;
using VContainer;

namespace Code.Game.Characters
{
    public class WorldCharacterRender : Essential.Mono, IDisposable
    {
        [field: SerializeField] public Transform Body { get; private set; }
        [field: SerializeField] public Collider2D Collider { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public WorldArmorRender WorldArmor { get; private set; }
        [field: SerializeField] public PathfindingAgentSettings AgentSettings { get; private set; }
        [field: SerializeField] public GameObject SelectView { get; set; }

        [field: SerializeField, ReadOnly] public string ID { get; private set; }
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
        
        public void SetActor(CharacterWorldActor actor)
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