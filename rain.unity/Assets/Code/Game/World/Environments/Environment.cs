using System;
using System.Collections.Generic;
using Code.Game.World.Turns;
using Code.Pathfinding;
using Essential;
using UnityEngine;
using VContainer;

namespace Code.Game.World
{
    public class Environment : WorldMono, IEntityView
    {
        public event Action<IEntityView, Vector3Int, Vector3Int> PositionChanged;
        [field: SerializeField] public string ID { get; private set;}
      
        [field: SerializeField] public EEntityType Type { get; private set;}
        [field: SerializeField] public Transform Body { get; private set;}
        [field: SerializeField] public Vector3Int CellPosition { get; private set; }
        [field: SerializeField] public bool IsOccupyingCell { get; private set;}
        [field: SerializeField] public EnvironmentHealth Health { get; private set; }
        [SerializeField] private MatrixZone _occupation;

        
        private ITilemapView _tilemapView;
        private IEntityRegistry _entityRegistry;


        [Inject]
        public void Construct(ITilemapView tilemapView, IEntityRegistry entityRegistry)
        {
            _tilemapView = tilemapView;
            _entityRegistry = entityRegistry;
        }

        public IReadOnlyList<Vector3Int> GetOccupiedCells()
        {
           return _occupation.GetCells();
        }

        private void OnEnable()
        {
            if (_entityRegistry == null)
            {
                return;
            }
            
            _entityRegistry.Register(this);
        }

        private void OnDisable()
        {
            if (_entityRegistry == null)
            {
                return;
            }

            _entityRegistry.UnRegister(this);
        }

        public void SetCellPosition(Vector3Int cellPosition)
        {
            Vector3Int previousPosition = CellPosition;
            CellPosition = cellPosition;
            Body.position = _tilemapView.CellAsCenterWorld(CellPosition);
            _occupation.UpdateOriginCell();
            PositionChanged?.Invoke(this, previousPosition, CellPosition);
        }

        public bool TryGetHealth(out IHealth health)
        {
            health = Health;

            return Health != null && Health.IsAlive && Health.MaxHealth > 0;
        }

        protected override void onSpanToTilemap(TilemapView tilemap, Vector3Int cell)
        {
            CellPosition = cell;
            
            base.onSpanToTilemap(tilemap, cell);
        }
    }
}