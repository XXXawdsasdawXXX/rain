using System;
using System.Collections.Generic;
using Code.Game.Save;
using Code.Game.StateMachine;
using Code.Game.StateMachine.States;
using Code.UI.Components;
using Code.UI.Components.Window;
using UnityEngine;

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
            _updateButtonsState();
        }

        protected override void subscribe()
        {
            _gamesSave.Changed += _updateView;

            view.Button_New.Clicked += _onClickedNewGame;
            view.Button_Delete.Clicked += _onClickedDeleteGame;
            view.Button_Play.Clicked += _onClickedPlay;
            view.DropDown_Games.Changed += _onChangedSelectedGame;
        }

        protected override void unSubscribe()
        {
            _gamesSave.Changed -= _updateView;

            view.Button_New.Clicked -= _onClickedNewGame;
            view.Button_Delete.Clicked -= _onClickedDeleteGame;
            view.Button_Play.Clicked -= _onClickedPlay;
            view.DropDown_Games.Changed -= _onChangedSelectedGame;
        }

        private void _onClickedNewGame()
        {
            _navigator.OpenNewGameWindow();
        }

        private void _onClickedDeleteGame()
        {
            _navigator.OpenDeleteWindow();
        }

        private void _onClickedPlay()
        {
            _stateMachine.RequestTransition(typeof(GameState));
        }

        private void _onChangedSelectedGame(int index)
        {
            if (view.DropDown_Games.IsInteractable)
            {
                _gamesSave.SetLastGameIndex(index);
            }
        }

        private void _updateView()
        {
            _updateButtonsState();
            _updateGamesDropDown();
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
            }
            else
            {
                options.Add("no games");
                view.DropDown_Games.SetCurrentWithoutNotify(0);
                return;
            }

            view.DropDown_Games.SetOptions(options);
            view.DropDown_Games.SetCurrentWithoutNotify(Math.Max(0, _gamesSave.Data.LastGameIndex));
            view.DropDown_Games.SetInteractable(_gamesSave.Data.LastGameIndex >= 0);
        }

        private void _updateButtonsState()
        {
            bool isHasSavedGames = _gamesSave.Data.Games.Count > 0;
            
            view.Button_Delete.SetInteractable(isHasSavedGames);
            view.Button_Play.SetInteractable(isHasSavedGames);
        }
    }
}