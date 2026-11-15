using System;
using System.Collections.Generic;
using Code.Game.Pathfinding.View;
using Essential;
using UnityEngine;
using VContainer;

namespace Code.Game.World.Environments
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

        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private MatrixZone _occupation;
        
        private ITilemapView _tilemapView;
        private IEntityRegistry _entityRegistry;


        [Inject]
        public void Construct(ITilemapView tilemapView, IEntityRegistry entityRegistry)
        {
            _tilemapView = tilemapView;
            _entityRegistry = entityRegistry;
            Log.Info(this, "[construct]");
        }
        
        public IReadOnlyList<Vector3Int> GetOccupiedCells()
        {
           return _occupation.GetCells();
        }

        private void OnEnable()
        {
            Log.Info(this, $"[on enable] {_entityRegistry != null}");
            if (_entityRegistry == null)
            {
                return;
            }
            
            _entityRegistry.Register(this);
            
            Health.ResetHealth();
            Health.IsDead.SubscribeProperty(_onDieChanged);
        }
        
        private void OnDisable()
        {
            Log.Info(this, $"[on disable] {_entityRegistry != null}");

            if (_entityRegistry == null)
            {
                return;
            }

            _entityRegistry.UnRegister(this);
            Health.IsDead.UnsubscribeProperty(_onDieChanged);
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
        
        private void _onDieChanged(bool died)
        {
            _renderer.color = died ? Color.gray : Color.white;
        }

#if UNITY_EDITOR
        protected override void e_onSpanToTilemap(TilemapView tilemap, Vector3Int cell)
        {
            CellPosition = cell;
            
            base.e_onSpanToTilemap(tilemap, cell);
        }
#endif

    }
}