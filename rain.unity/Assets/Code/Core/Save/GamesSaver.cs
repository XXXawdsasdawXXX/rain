using Core.Save.Models;

namespace Core.Save
{
    public class GamesSaver : Saver<GameModel>
    {
        protected override GameModel getNewModel()
        {
            return new GameModel();
        }
    }
}