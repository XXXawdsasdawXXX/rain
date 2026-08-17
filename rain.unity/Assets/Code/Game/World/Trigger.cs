using System;
using System.Collections.Generic;
using Essential.Extensions;
using UnityEngine;

namespace Code.Game.World
{
    public class Trigger : Essential.Mono
    {
        public event Action<Collider2D, bool> Changed; 
        public List<Collider2D> Inside { get; } = new();  
        
        [SerializeField] private Collider2D _collider;

        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!Inside.Contains(other))
            {
                Inside.Add(other);
                Changed?.Invoke(other, true);
            }

            Debug.Log($"[OnTriggerEnter2D] {other.name}");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (Inside.Contains(other))
            {
                Inside.Remove(other);
                Changed?.Invoke(other, false);
            }

            Debug.Log($"[OnTriggerExit2D] {other.name}");
        }

        public void Dispose()
        {
            Inside.Clear();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = ColorExtension.OBSTACLE_GIZMO_COLOR;

            if (_collider != null)
            {
                
            }
        }
#endif
    }
}