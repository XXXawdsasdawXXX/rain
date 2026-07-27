using System.Collections.Generic;
using Essential;
using UnityEngine;

namespace Code.Pathfinding
{
    public class PathView : Essential.Mono
    {
        [SerializeField] private Transform _selectedTileBody;
        [SerializeField] private SpriteRenderer _selectTileRender;
        [Space]
        [SerializeField] private MonoPool<PathfindingPointView> _pathPointsPool;

        
        public void SetSelectTile(Vector3Int position)
        {
            _selectedTileBody.position = position;
        }
        
        public void SetPath(List<Vector3Int> path, bool isWalkable)
        {
            _pathPointsPool.DisableAll();
            _selectTileRender.color = isWalkable ? Color.white : Color.red;

            foreach (Vector3Int vector3Int in path)
            {
                PathfindingPointView point = _pathPointsPool.GetNext();
                point.transform.position = vector3Int;
            }
        }
    }
}