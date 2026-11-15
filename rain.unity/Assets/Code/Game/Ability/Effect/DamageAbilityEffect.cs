using Code.Game.Player.Control;
using Code.Game.World;

namespace Code.Game.Ability.Effect
{
    public class DamageAbilityEffect : IAbilityEffect
    {
        public void Apply(AbilityOutcome outcome)
        {
            float damage = outcome.Ability.GetDamage();
            if (damage < 0)
            {
                return;
            }
            foreach (IHealth target in outcome.Targets)
            {
                target.TakeDamage(damage);
            }
        }
    }
}