using Code.CoreGame.Entities.Animation;

namespace Code.CoreGame.Entities.Characters.Interfaces
{
    public interface IHarvestAnimator
    {
        void StartMine(AnimatorKey.EHarvestType harvestType);
        void StopMine();
    }
}