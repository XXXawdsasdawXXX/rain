using Code.Game.Characters;
using Code.Game.Save;

namespace Code.Game.Factories
{
    public interface ICharacterFactory
    {
        CharacterActor SpawnHero(CharacterModel model);
        CharacterActor SpawnNpc(CharacterModel model);
    }
}