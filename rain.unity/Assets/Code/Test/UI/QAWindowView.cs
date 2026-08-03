using Code.Game.Save;
using Code.Game.StateMachine;
using Code.Game.StateMachine.States;
using Code.UI.Components;
using Code.UI.Components.Window;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Test.UI
{
    public class QAWindowView : AUIWindowView
    {
        [field: SerializeField] public UIButton ButtonOpenWindow { get; private set; }
        [field: SerializeField] public UIButton ButtonCloseWindow { get; private set; }
        [field: SerializeField] public UIButton ButtonSave { get; private set; }
        [field: SerializeField] public UIButton ButtonMainMenu { get; private set; }
    }

    
    [UsedImplicitly]
    public class QAWindowController : AUIWindowController<QAWindowView>
    {
        private readonly GameSaveRepository _gameSave;
        private readonly GameStateMachine _gameStateMachine;


        public QAWindowController(QAWindowView view, GameSaveRepository gameSave, GameStateMachine gameStateMachine) : base(view)
        {
            _gameSave = gameSave;
            _gameStateMachine = gameStateMachine;
        }

        protected override void subscribe()
        {
            view.ButtonOpenWindow.Clicked += view.Open;
            view.ButtonCloseWindow.Clicked += view.Close;

            view.ButtonSave.Clicked += _saveGame;
            view.ButtonMainMenu.Clicked += _loadMainMenu;
        }

        protected override void unSubscribe()
        {
            view.ButtonOpenWindow.Clicked -= view.Open;
            view.ButtonCloseWindow.Clicked -= view.Close;
            
            view.ButtonSave.Clicked -= _saveGame;
            view.ButtonMainMenu.Clicked -= _loadMainMenu;
        }

        private async void _saveGame()
        {
            await _gameSave.SaveAsync();
        }

        private void _loadMainMenu()
        {
            _gameStateMachine.RequestTransition(typeof(MenuState));
        }
    }
}