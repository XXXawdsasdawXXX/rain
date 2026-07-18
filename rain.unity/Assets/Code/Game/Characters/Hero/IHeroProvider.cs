using System;

namespace Code.Game.Characters
{
    public interface IHeroProvider
    {
        public event Action Created;
        CharacterView Hero { get; }
        bool HasHero => Hero != null;
        void Set(CharacterView character);
    }
}