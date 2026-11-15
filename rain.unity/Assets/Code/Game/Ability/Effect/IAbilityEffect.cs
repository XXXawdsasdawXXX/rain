using Code.Game.Player.Control;

namespace Code.Game.Ability.Effect
{
    public interface IAbilityEffect
    {
        void Apply(AbilityOutcome outcome);
    }
}