using UnityEngine;

namespace Essential.Extensions
{
    public static class VectorExtension
    {
        /// <summary>World size of one map/trigger cell (matches Grid Cell Size).</summary>
        public const float CELL_SIZE = 0.64f;

        /// <summary>
        /// Bottom-left local corner of a W×H cell matrix centered on the object.
        /// </summary>
        public static Vector2 GetCenteredMatrixOrigin(int widthCells, int heightCells)
        {
            return new Vector2(
                -widthCells * CELL_SIZE * 0.5f,
                -heightCells * CELL_SIZE * 0.5f);
        }

        public static Position AsPosition(this Vector3 worldPosition)
        {
            return new Position(worldPosition.x, worldPosition.y);
        }

        public static Vector2 AsVector2(this float value)
        {
            return new Vector2(value, value);
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
