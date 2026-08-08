using Code.Game.StateMachine;
using Code.UI.Components;
using Code.UI.Components.Window;
using Core.Input;
using UnityEngine;

namespace Code.UI.Hud.Windows
{
    public class PauseWindowView : AUIWindowView
    {
        [field: SerializeField] public UIButton ButtonOpen { get; private set; }
        [field: SerializeField] public UIButton ButtonClose { get; private set; }
    }

    public class PauseWindowController : AUIWindowController<PauseWindowView>
    {
        private readonly IInputService _input;
        private readonly IGamePhaseService _gamePhase;


        public PauseWindowController(
            PauseWindowView view, 
            IInputService input, 
            IGamePhaseService gamePhase) : base(view)
        {
            _input = input;
            _gamePhase = gamePhase;
        }

        protected override void subscribe()
        {
            _input.ActionEnded += _onInputActionEnded;

            view.ButtonOpen.Clicked += _open;
            view.ButtonClose.Clicked += _close;
        }

        protected override void unSubscribe()
        {
            _input.ActionEnded -= _onInputActionEnded;
            
            view.ButtonOpen.Clicked -= _open;
            view.ButtonClose.Clicked -= _close;
        }

        private void _onInputActionEnded(EInputAction action)
        {
            if (action is EInputAction.Esc)
            {
                if (_gamePhase.Phase is EGamePhase.Paused && view.IsOpened)
                {
                    _close();
                }
                else if(_gamePhase.Phase is not EGamePhase.Paused)
                {
                    _open();
                }
            }
        }

        private void _close()
        {
            view.Close();
            _gamePhase.TogglePause();
        }

        private void _open()
        {
            view.Open();
            _gamePhase.TogglePause();
        }
    }
}