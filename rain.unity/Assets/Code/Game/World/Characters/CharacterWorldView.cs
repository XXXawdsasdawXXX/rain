using System;
using Code.Game.World;
using Code.Pathfinding;
using UnityEngine;
using VContainer;

namespace Code.Game.Characters
{
    public class CharacterWorldView : MonoBehaviour, IEntityView
    {
        public event Action<IEntityView, Vector3Int, Vector3Int> PositionChanged;
        
        public string ID => _actor != null ? _actor.Model.ID : string.Empty;
        public Vector3Int CellPosition { get; private set; }
        [field: SerializeField] public Transform Body { get; private set; }

        [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }
        [field: SerializeField] public bool IsOccupyingCell { get; set; }
        [field: SerializeField] public CharacterWorldRender Render { get; private set; }
        [field: SerializeField] public EEntityType Type { get; private set; }

        private CharacterActor _actor;
        private ITilemapView _tilemap;


        [Inject]
        public void Construct(ITilemapView tilemap)
        {
            _tilemap = tilemap;
        }
        
        public void SetCellPosition(Vector3Int cellPosition)
        {
            Vector3Int lastPosition = CellPosition;
            Body.position = _tilemap.CellAsCenterWorld(cellPosition);
            CellPosition = cellPosition;
            PositionChanged?.Invoke(this, lastPosition, cellPosition);
        }

        public bool TryGetHealth(out IHealth health)
        {
            health = _actor;
            return health != null;
        }

        public void SetActor(CharacterActor actor)
        {
            _actor = actor;
        }
        
        public void SetEntityType(EEntityType type)
        {
            Type = type;
        }
    }
}