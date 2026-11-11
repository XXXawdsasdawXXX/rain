using System;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using Code.Pathfinding;
using TriInspector;
using UnityEditor;
#endif

namespace Essential
{
    public abstract class WorldMono : MonoBehaviour, IEquatable<WorldMono>
    {
        public bool Equals(WorldMono other)
        {
            return other != null && other.gameObject.GetEntityId().Equals(GetEntityId());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WorldMono)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

#if UNITY_EDITOR
        
        [Button]
        private void _snapToTilemap()
        {
            TilemapView tilemap = FindAnyObjectByType<TilemapView>();

            if (tilemap == null)
            {
                throw new Exception("Tilemap is null");
            }

            Vector3Int cellPosition = tilemap.WorldAsCell(transform.position);
            Vector3 centerWorld = tilemap.CellAsCenterWorld(cellPosition);

            transform.position = centerWorld;
            
            onSpanToTilemap(tilemap);
            
            EditorUtility.SetDirty(this);
        }

        protected virtual void onSpanToTilemap(TilemapView tilemap)
        {
            
        }
        
#endif
    }
}