using Code.Game.Characters;
using Code.Game.Save;

namespace Code.Game.Factories
{
    public interface ICharacterFactory
    {
        CharacterView SpawnHero(CharacterModel model);
        CharacterView SpawnNpc(CharacterModel model);
    }
}