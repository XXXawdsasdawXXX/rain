using System;
using TriInspector;

namespace Essential.Data
{
    [Serializable]
    public struct UniqueID
    {
        [ReadOnly, ShowInInspector] public string Value { get; private set; }
        
        
        [Button]
        public void Validate()
        {
            if (string.IsNullOrEmpty(Value))
            {
                Value = Guid.NewGuid().ToString();
            }
        }
    }
}