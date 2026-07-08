using System;
using System.Collections.Generic;
using System.Linq;
using Core.Save;

namespace Code.Game.Save
{
    public sealed class GameSaveRepository : ASaveRepository<GameSaveRepository.Model>
    {
        [Serializable]
        public class Model
        {
            public List<GameModel> Games;
            public int LastGameIndex;
        }

        public event Action Changed;
        
        
        public GameSaveRepository(ISaveProcessor processor) : base(processor) { }

        public bool GetLastGame(out GameModel game)
        {
            if (Data.LastGameIndex < 0 || Data.LastGameIndex >= Data.Games.Count)
            {
                Data.LastGameIndex = -1;
            }
            
            if (Data.Games.Count > 0)
            {
                Data.LastGameIndex = _getLastGameIndex();
            }

            game = Data.LastGameIndex >= 0 
                ? Data.Games[Data.LastGameIndex] 
                : null;
            
            return game != null;
        }

        public void AddNewGame(HeroModel heroModel, WorldModel worldModel)
        {
            GameModel newGame = new()
            {
                ID = Guid.NewGuid().ToString(),
                Hero = heroModel,
                World = worldModel,
                GameTime = TimeSpan.Zero,
            };

            Data.Games.Add(newGame);
            Data.LastGameIndex = Data.Games.Count - 1;

            WasChanged = true;
            
            Changed?.Invoke();
        }

        public void DeleteGame(int gameIndex)
        {
            GameModel game = Data.Games[gameIndex];

            if (game != null)
            {
                Data.Games.Remove(game);
            }

            if (Data.LastGameIndex == gameIndex)
            {
                if (Data.Games.Count > 0)
                {
                    Data.LastGameIndex = _getLastGameIndex();
                }
                else
                {
                    Data.LastGameIndex = -1;
                }
            }
            
            WasChanged = true;

            Changed?.Invoke();
        }

        public void SetLastGameIndexWithoutNotify(int index)
        {
            Data.LastGameIndex = index;
            
            WasChanged = true;
        }

        private int _getLastGameIndex()
        {
            TimeSpan currentTime = DateTime.UtcNow.TimeOfDay;
            
            return Data.Games.IndexOf(Data.Games
                .OrderBy(w => Math.Abs((currentTime - w.ExitTime).Ticks))
                .FirstOrDefault());
        }

        protected override Model createDefault()
        {
            return new Model
            {
                Games = new List<GameModel>(),
                LastGameIndex = -1
            };
        }
    }
}