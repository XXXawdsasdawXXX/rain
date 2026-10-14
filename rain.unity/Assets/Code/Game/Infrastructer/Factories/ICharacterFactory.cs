using Code.Game.Characters;
using Code.Game.Save;

namespace Code.Game.Factories
{
    public interface ICharacterFactory
    {
        CharacterWorldActor SpawnHero(CharacterModel model);
        CharacterWorldActor SpawnNpc(CharacterModel model);
    }
}