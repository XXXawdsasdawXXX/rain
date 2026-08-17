using UnityEngine;

namespace Code.Game.World
{
    public class TriggersArea<T> : Essential.Mono
    {
        [SerializeField] private Trigger[] _triggers;
    }
}