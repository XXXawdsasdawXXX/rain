using System;
using System.Collections.Generic;
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
            IReadOnlyList<Vector3Int> occupiedCells = entity.GetOccupiedCells();
            foreach (Vector3Int cell in occupiedCells)
            {
                if (_entities.TryGetValue(cell, out List<IEntityView> list))
                {
                    if (list.Contains(entity))
                    {
                        throw new Exception($"[Register] collection already contains entity by coordinate {entity.CellPosition}");
                    }

                    list.Add(entity);
                }
                else
                {
                    _entities.Add(cell, new List<IEntityView>()
                    {
                        entity
                    });
                }
                
                entity.PositionChanged += _onEntityPositionChanged;
            }
        }

        public void UnRegister(IEntityView entity)
        {
            IReadOnlyList<Vector3Int> occupiedCells = entity.GetOccupiedCells();
            foreach (Vector3Int cell in occupiedCells)
            {
                if (_entities.TryGetValue(cell, out List<IEntityView> list))
                {
                    if (list.Contains(entity))
                    {
                        list.Remove(entity);
                        entity.PositionChanged -= _onEntityPositionChanged;
                    }
                    else
                    {
                        throw new Exception($"[UnRegister] collection has not entity({entity.ID}) in {entity.CellPosition}");
                    }
                }
                else
                {
                    throw new Exception($"[UnRegister] collection has not any entities in {entity.CellPosition}");
                }
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
            }

            if (_entities.TryGetValue(to, out List<IEntityView> toList))
            {
                if (toList.Contains(entity))
                {
                    throw new Exception($"[_onEntityPositionChanged] collection already has entities in {from}");
                }

                toList.Add(entity);
             
                return;
            }
            
            _entities.Add(to, new List<IEntityView>() { entity });
        }
    }
}