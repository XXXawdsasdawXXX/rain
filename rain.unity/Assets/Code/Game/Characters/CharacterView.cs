using System;
using System.Reflection.Emit;
using Code.Game.Characters.Armor;
using Code.Game.Save;
using Code.Pathfinding;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Game.Characters
{
    public class CharacterView : Essential.Mono, IStartable, IDisposable
    {
        [field: SerializeField] public Transform Body { get; private set; }
        [field: SerializeField] public Collider2D Collider { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public ArmorRender ArmorRender { get; private set; }
        [field: SerializeField] public PathfindingAgentSettings AgentSettings { get; private set; }
        
        public CharacterModel Model { get; private set; }

        private ArmoryLibrary _armoryLibrary;

        
        [Inject]
        public void Construct(ArmoryLibrary library)
        {
            _armoryLibrary = library;
        }

        public void SetModel(CharacterModel model)
        {
            Model = model;
        }

        public void Start()
        {
            Model.ArmorChanged += _updateArmorRender;

            foreach ((EBodyPart part, int setIndex) in Model.Armor)
            {
                _updateArmorRender(part, setIndex);
            }
        }

        public void Dispose()
        {
            Model.ArmorChanged -= _updateArmorRender;
        }

        private void _updateArmorRender(EBodyPart part, int index)
        {
            ArmorPieceModel armorPiece = _armoryLibrary.Armory.Get(index).Pieces[(int)part];
            ArmorRender.SetArmorPiece(armorPiece);
        }
    }
}