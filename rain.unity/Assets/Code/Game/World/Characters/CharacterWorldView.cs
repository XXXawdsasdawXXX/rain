using Code.Game.World;
using Code.Pathfinding;
using UnityEngine;

namespace Code.Game.Characters
{
    public class CharacterWorldView : MonoBehaviour, IWorldEntityView
    {
        public string ID => _actor != null ? _actor.Model.ID : string.Empty;
        
        [field: SerializeField] public Transform Body { get; private set; }
        [field: SerializeField] public PathfindingAgentSettings AgentSettings { get; private set; }
        
        [field: SerializeField] public CharacterWorldRender Render { get; private set; }
        [field: SerializeField] public EEntityType Type { get; private set; }

        private CharacterWorldActor _actor;

        
        public bool TryGetHealth(out IHealth health)
        {
            health = _actor;
            return health != null;
        }

        public void SetActor(CharacterWorldActor actor)
        {
            _actor = actor;
        }
        
        public void SetEntityType(EEntityType type)
        {
            Type = type;
        }
    }
}