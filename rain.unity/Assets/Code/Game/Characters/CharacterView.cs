using UnityEngine;

namespace Code.Game.Characters
{
    public class CharacterView : Essential.Mono
    {
        [field: SerializeField] public Transform Body { get; private set; }
        [field: SerializeField] public Collider2D Collider { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public ArmorRender ArmorRender { get; private set; }
    }
}