using UnityEngine;

namespace Code.Game.World
{
    public class TriggersArea<T> : WorldMono
    {
        [SerializeField] private Trigger[] _triggers;
    }
}