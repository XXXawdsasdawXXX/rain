using System.Collections.Generic;
using Code.UI.Components.Window;
using JetBrains.Annotations;

namespace Code.UI.Menu
{
    [UsedImplicitly]
    public class MenuNavigator : AUIWindowNavigator
    {
        private readonly DeleteGameWindowView _deleteGame;
        private readonly NewGameWindowView _newGame;
        
        
        public MenuNavigator(IEnumerable<AUIWindowView> windows) : base(windows)
        {
            _deleteGame = getView<DeleteGameWindowView>();
            _newGame = getView<NewGameWindowView>();
        }

        public void OpenDeleteWindow()
        {
            if (_newGame.IsOpened)
            {
                _newGame.Close();
            }

            _deleteGame.Open();
        }

        public void OpenNewGameWindow()
        {
            if (_deleteGame.IsOpened)
            {
                _deleteGame.Close();
            }
            
            _newGame.Open();
        }
    }
}