using System;
using UnityEngine;

namespace Essential
{
    public abstract class Mono : MonoBehaviour, IEquatable<Mono>
    {
        public bool Equals(Mono other)
        {
            return other != null && other.gameObject.GetEntityId().Equals(GetEntityId());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Mono)obj);
        }
    }
}