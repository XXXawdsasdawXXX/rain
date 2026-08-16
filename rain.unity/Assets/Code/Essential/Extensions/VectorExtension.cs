using UnityEngine;

namespace Essential.Extensions
{
    public static class VectorExtension
    {
        public static Position AsPosition(this Vector3 worldPosition)
        {
            return new Position(worldPosition.x, worldPosition.y);
        }

        public static Vector3 AsVector3(this Position position)
        {
            return new Vector3(position.X, position.Y, 0);
        }
        
        public static Vector3 AsVector3(this Vector2 position)
        {
            return new Vector3(position.x, position.y, 0);
        }
    }
}