using Unity.Mathematics;
using UnityEngine;

namespace Core.Common.Extensions
{
    public static class VectorExtension
    {
        #region to vector2

        public static Vector2 AsVector2(this float2 vector)
        {
            return new(vector.x, vector.y);
        }

        #endregion

        #region to vector3

        public static Vector3 AsVector3(this Vector2 vector)
        {
            return new(vector.x, vector.y, 0);
        }

        public static Vector3 AsVector3(this float2 vector)
        {
            return new(vector.x, vector.y, 0);
        }

        #endregion


        #region to float2

        public static float2 AsFloat2(this Vector2 vector)
        {
            return new(vector.x, vector.y);
        }

        public static float2 AsFloat2(this Vector2Int vector)
        {
            return new(vector.x, vector.y);
        }

        public static float2 AsFloat2(this Vector3 vector)
        {
            return new(vector.x, vector.y);
        }

        public static float2 AsFloat2(this Vector3Int vector)
        {
            return new(vector.x, vector.y);
        }

        public static string AsString(this float2 pos)
        {
            return $"{pos.x:F2}_{pos.y:F2}";
        }

        public static float2 AsFloat2(this string key)
        {
            string[] parts = key.Split('_');
            return new float2(float.Parse(parts[0]), float.Parse(parts[1]));
        }

        #endregion
    }
}