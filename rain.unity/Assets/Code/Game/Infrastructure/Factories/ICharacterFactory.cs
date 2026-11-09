using Code.Game.Characters;
using Code.Game.Save;
using Code.Game.World;

namespace Code.Game.Factories
{
    public interface ICharacterFactory
    {
        CharacterActor Spawn(EEntityType type, CharacterModel model);
        CharacterActor SpawnHero(CharacterModel model);
        CharacterActor SpawnNpc(CharacterModel model);
    }
}