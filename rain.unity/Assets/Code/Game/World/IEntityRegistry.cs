using System.Collections.Generic;
using Essential;
using UnityEngine;

namespace Code.Game.World.Turns
{
    public interface IEntityRegistry : IRegister<IWorldEntityView>
    {
        bool TryGetEntities(Vector3Int cellPosition, out List<IWorldEntityView> list);
    }  
    
    public class EntityRegistry : IEntityRegistry
    {
        private readonly Dictionary<Vector3Int, List<IWorldEntityView>> _entities = new();

        public void Register(IWorldEntityView entity)
        {
            if (_entities.TryGetValue(entity.CellPosition, out List<IWorldEntityView> list))
            {
                if (list.Contains(entity))
                {
                    Log.Error(this, $"[Register] collection already contains entity by coordinate {entity.CellPosition}");
            
                    return;
                }
            }
            else
            {
                _entities.Add(entity.CellPosition, new List<IWorldEntityView>()
                {
                    entity
                });
            }
            
            entity.PositionChanged += _onEntityPositionChanged;
        }

        public void UnRegister(IWorldEntityView entity)
        {
            if (_entities.TryGetValue(entity.CellPosition, out List<IWorldEntityView> list))
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

        public bool TryGetEntities(Vector3Int cellPosition, out List<IWorldEntityView> list)
        {
            return _entities.TryGetValue(cellPosition, out list);
        }

        private void _onEntityPositionChanged(IWorldEntityView entity, Vector3Int from, Vector3Int to)
        {
            if (_entities.TryGetValue(from, out List<IWorldEntityView> list))
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

            _entities.Add(to, new List<IWorldEntityView>() { entity });
        }
    }
}