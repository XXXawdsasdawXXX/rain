using Essential;

namespace Code.Pathfinding
{
    public class PathfindingPointView : Essential.Mono, IPoolEntity
    {
        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}