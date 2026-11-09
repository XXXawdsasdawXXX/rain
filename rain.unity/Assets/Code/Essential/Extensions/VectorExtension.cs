using UnityEngine;

namespace Essential.Extensions
{
    public static class VectorExtension
    {
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
    }
}
