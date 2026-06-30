using System;
using UnityEngine;

namespace Core.Input
{
    [Serializable]
    public struct InputActionKey
    {
        public KeyCode Key;
        public EInputAction Action;
    }
}