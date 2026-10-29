using System;
using System.Collections.Generic;
using System.Linq;
using Core.Save;
using JetBrains.Annotations;

namespace Code.Game.Save
{
    [UsedImplicitly]
    public sealed class GameSaveRepository : ASaveRepository<GameSaveRepository.Model>
    {
        [Serializable]
        public class Model
        {
            public List<GameModel> Games;
            public int LastGameIndex;
        }

        public event Action Changed;


        public GameSaveRepository(ISaveProcessor processor) : base(processor)
        {
            
        }

        public bool TryGetLastGame(out GameModel game)
        {
            if (Data.LastGameIndex < 0 || Data.LastGameIndex >= Data.Games.Count)
            {
                Data.LastGameIndex = -1;
            }
            
            if (Data.Games.Count > 0 && Data.LastGameIndex < 0)
            {
                Data.LastGameIndex = _getLastGameIndex();
            }

            game = Data.LastGameIndex >= 0 
                ? Data.Games[Data.LastGameIndex] 
                : null;
            
            return game != null;
        }

        public GameModel GetCurrentGame()
        {
            return Data.Games[Data.LastGameIndex];
        }
        
        public void AddNewGame(CharacterModel characterModel, WorldModel worldModel)
        {
            GameModel newGame = new()
            {
                ID = Guid.NewGuid().ToString(),
                Heroes = new List<CharacterModel> { characterModel },
                Npc = new List<CharacterModel>(),
                World = worldModel,
                GameTime = TimeSpan.Zero,
            };

            Data.Games.Add(newGame);
            Data.LastGameIndex = Data.Games.Count - 1;

            WasChanged = true;
            
            Changed?.Invoke();
        }

        public void DeleteLastGame()
        {
            _deleteGame(Data.LastGameIndex);
        }

        public void SetLastGameIndex(int index)
        {
            Data.LastGameIndex = index;
            
            WasChanged = true;
            
            Changed?.Invoke();
        }

        protected override Model createDefault()
        {
            return new Model
            {
                Games = new List<GameModel>(),
                LastGameIndex = -1
            };
        }

        private void _deleteGame(int gameIndex)
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

        private int _getLastGameIndex()
        {
            TimeSpan currentTime = DateTime.UtcNow.TimeOfDay;
            
            return Data.Games.IndexOf(Data.Games
                .OrderBy(w => Math.Abs((currentTime - w.ExitTime).Ticks))
                .FirstOrDefault());
        }
    }
}