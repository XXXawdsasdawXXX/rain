using System;
using UnityEngine;

namespace Code.Game.World
{
    public interface IEntityView
    {
        public event Action<IEntityView, Vector3Int, Vector3Int> PositionChanged; 

        public string ID { get; }
        public EEntityType Type { get; }
        public Transform Body { get; }
        Vector3Int CellPosition { get; }
        bool IsOccupyingCell { get; }

        public void SetCellPosition(Vector3Int cellPosition);

        public bool TryGetHealth(out IHealth health);
    }
}