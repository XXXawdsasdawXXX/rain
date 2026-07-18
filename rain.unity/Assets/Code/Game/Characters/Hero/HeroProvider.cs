using System;
using JetBrains.Annotations;

namespace Code.Game.Characters
{
    [UsedImplicitly]
    public sealed class HeroProvider : IHeroProvider
    {
        public event Action Created;
        public CharacterView Hero { get; private set; }

        
        public void Set(CharacterView character)
        {
            Hero = character;
            Created?.Invoke();
        }
    }
}