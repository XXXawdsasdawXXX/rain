using Code.Game.Save;
using Code.Pathfinding;
using UnityEngine;

namespace Code.Game.Characters
{
    public class CharacterView : Essential.Mono
    {
        [field: SerializeField] public Transform Body { get; private set; }
        [field: SerializeField] public Collider2D Collider { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public ArmorRender ArmorRender { get; private set; }
        [field: SerializeField] public PathfindingAgentSettings AgentSettings { get; private set; }
        
        public CharacterModel Model { get; private set; }


        public void SetModel(CharacterModel model)
        {
            Model = model;
        }
    }
}