using Code.Game.Characters;
using Code.Game.Save;

namespace Code.Game.Factories
{
    public interface IHeroFactory
    {
        CharacterView SpawnHero(CharacterModel model);
    }
}