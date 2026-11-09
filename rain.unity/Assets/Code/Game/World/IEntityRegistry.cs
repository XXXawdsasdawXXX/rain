using System.Collections.Generic;
using Essential;
using UnityEngine;

namespace Code.Game.World.Turns
{
    public interface IEntityRegistry : IRegister<IEntityView>
    {
        bool TryGetEntities(Vector3Int cellPosition, out List<IEntityView> list);
        bool IsEmptyCell(Vector3Int cellPosition);
    }  
    
    public class EntityRegistry : IEntityRegistry
    {
        private readonly Dictionary<Vector3Int, List<IEntityView>> _entities = new();

        public void Register(IEntityView entity)
        {
            if (_entities.TryGetValue(entity.CellPosition, out List<IEntityView> list))
            {
                if (list.Contains(entity))
                {
                    Log.Error(this, $"[Register] collection already contains entity by coordinate {entity.CellPosition}");
            
                    return;
                }
            }
            else
            {
                _entities.Add(entity.CellPosition, new List<IEntityView>()
                {
                    entity
                });
            }
            
            entity.PositionChanged += _onEntityPositionChanged;
        }

        public void UnRegister(IEntityView entity)
        {
            if (_entities.TryGetValue(entity.CellPosition, out List<IEntityView> list))
            {
                if (list.Contains(entity))
                {
                    list.Remove(entity);
                    entity.PositionChanged -= _onEntityPositionChanged;
                }
                else
                {
                    Log.Error(this, $"[UnRegister] collection has not entity({entity.ID}) in {entity.CellPosition}");
                }
            }
            else
            {
                Log.Error(this, $"[UnRegister] collection has not any entities in {entity.CellPosition}");
            }
        }

        public bool TryGetEntities(Vector3Int cellPosition, out List<IEntityView> list)
        {
            return _entities.TryGetValue(cellPosition, out list);
        }

        public bool IsEmptyCell(Vector3Int cellPosition)
        {
            if (TryGetEntities(cellPosition, out List<IEntityView> list))
            {
                return !list.Exists(e => e.IsOccupyingCell);
            }

            return true;
        }
        
        private void _onEntityPositionChanged(IEntityView entity, Vector3Int from, Vector3Int to)
        {
            if (_entities.TryGetValue(from, out List<IEntityView> list))
            {
                if (list.Contains(entity))
                {
                    list.Remove(entity);
                }
                else
                {
                    Log.Error(this, $"[_onEntityPositionChanged] collection has not entities in {from}");
                    return;
                }
            }

            _entities.Add(to, new List<IEntityView>() { entity });
        }
    }
}