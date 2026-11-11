using UnityEngine;

namespace Code.Game.World
{
    public class TriggersArea<T> : Essential.WorldMono
    {
        [SerializeField] private Trigger[] _triggers;
    }
}