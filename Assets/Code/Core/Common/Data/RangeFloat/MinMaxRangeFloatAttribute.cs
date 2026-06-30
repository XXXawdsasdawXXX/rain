using System;

namespace Core.Common.Data.RangeFloat
{
    public class MinMaxRangeFloatAttribute : Attribute
    {
        public MinMaxRangeFloatAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Min { get; }
        public float Max { get; }
    }
}