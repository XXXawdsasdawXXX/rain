using Code.Game.Player.Control;
using Code.Game.World;

namespace Code.Game.Ability.Effect
{
    public class HealAbilityEffect : IAbilityEffect
    {
        public void Apply(AbilityOutcome outcome)
        {
            float heal = outcome.Ability.GetHeal();
            if (heal < 0)
            {
                return;
            }
            foreach (IHealth target in outcome.Targets)
            {
                target.Heal(heal);
            }
        }
    }
}