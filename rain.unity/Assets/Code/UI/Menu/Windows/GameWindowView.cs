using System.Collections.Generic;
using Code.Game.Save;
using Code.Game.StateMachine;
using Code.Game.StateMachine.States;
using Code.UI.Components;
using Code.UI.Components.Window;
using UnityEngine;
using VContainer.Unity;

namespace Code.UI.Menu
{
    public sealed class GameWindowView : AUIWindowView
    {
        [field: SerializeField] public UIButton Button_Play { get; private set; }
        [field: SerializeField] public UIButton Button_New { get; private set; }
        [field: SerializeField] public UIButton Button_Delete { get; private set; }
        [field: SerializeField] public UIDropDown DropDown_Games { get; private set; }
    }

    public sealed class GameWindowController : AUIWindowController<GameWindowView>
    {
        private readonly GameSaveRepository _gamesSave;
        private readonly GameStateMachine _stateMachine;
        private readonly MenuNavigator _navigator;


        public GameWindowController(GameWindowView view, 
            GameSaveRepository gamesSave, 
            GameStateMachine stateMachine,
            MenuNavigator navigator) : base(view)
        {
            _gamesSave = gamesSave;
            _stateMachine = stateMachine;
            _navigator = navigator;
        }

        public override void Start()
        {
            base.Start();
            
            _updateGamesDropDown();
        }

        protected override void subscribe()
        {
            _gamesSave.Changed += _updateGamesDropDown;
            
            view.Button_New.Clicked += _onClickedNewGame;
            view.Button_Delete.Clicked += _onClickedDeleteGame;
            view.Button_Play.Clicked += _onClickedPlay;
            view.DropDown_Games.Changed += _onChangedSelectedGame;
        }

        protected override void unSubscribe()
        {
            view.Button_New.Clicked += _onClickedNewGame;
            view.Button_Play.Clicked -= _onClickedPlay;
        }

        private void _onClickedNewGame()
        {
            _navigator.OpenNewGameWindow();
        }

        private void _onClickedDeleteGame()
        {
            if (_gamesSave.Data.LastGameIndex >= 0)
            {
                _navigator.OpenDeleteWindow(_gamesSave.Data.LastGameIndex);
            }
        }

        private void _onClickedPlay()
        {
            _stateMachine.RequestTransition(typeof(GameState));
        }

        private void _onChangedSelectedGame(int index)
        {
            _gamesSave.SetLastGameIndexWithoutNotify(index);
        }

        private void _updateGamesDropDown()
        {
            List<string> options = new();
            
            if (_gamesSave.Data.Games.Count > 0)
            {
                foreach (GameModel gameModel in _gamesSave.Data.Games)
                {
                    options.Add($"{gameModel.Hero.Name}");
                }
                
                view.DropDown_Games.SetInteractable(true);
            }
            else
            {
                options.Add("no games");
                view.DropDown_Games.SetInteractable(false);
                return;
            }

            view.DropDown_Games.SetOptions(options);
            if (_gamesSave.Data.LastGameIndex >= 0)
            {
                view.DropDown_Games.SetCurrentWithoutNotify(_gamesSave.Data.LastGameIndex);
            }
        }
    }
}