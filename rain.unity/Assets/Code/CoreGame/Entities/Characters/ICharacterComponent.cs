using Core.Common.Data;

namespace Code.CoreGame.Entities.Characters
{
    public interface ICharacterComponent
    {
        Condition Condition { get; }
    }
}