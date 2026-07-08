using System.Collections.Generic;
using Code.UI.Components.Window;

namespace Code.UI.Menu
{
    public class MenuNavigator : AUIWindowNavigator
    {
        private readonly DeleteGameWindowView _deleteGame;
        private readonly NewGameWindowView _newGame;
        
        
        public MenuNavigator(IEnumerable<AUIWindowView> windows) : base(windows)
        {
            _deleteGame = getView<DeleteGameWindowView>();
            _newGame = getView<NewGameWindowView>();
        }

        public void OpenDeleteWindow(int contextGameIndex)
        {
            if (_newGame.IsOpened)
            {
                _newGame.Close();
            }
            
            _deleteGame.SetNewModel(new DeleteGameWindowView.Model
            {
                ContextGameIndex = contextGameIndex
            });
            
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