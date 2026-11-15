using Code.Game.Infrastructure.Save;
using Code.Game.World;
using Code.Game.World.Characters;

namespace Code.Game.Infrastructure.Factories
{
    public interface ICharacterFactory
    {
        CharacterActor Spawn(EEntityType type, CharacterModel model);
        CharacterActor SpawnHero(CharacterModel model);
        CharacterActor SpawnNpc(CharacterModel model);
    }
}