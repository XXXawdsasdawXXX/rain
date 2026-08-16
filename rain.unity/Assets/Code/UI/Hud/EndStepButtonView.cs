using Code.Game.StateMachine;
using Code.UI.Components;
using Code.UI.Components.Window;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Hud
{
    public sealed class EndStepButtonView : AUIWindowView
    {
        [field: SerializeField] public UIButton ButtonEndStep;
        [field: SerializeField] public Image ImageBackground;
        [field: SerializeField] public UIText TextHeader;
    }

    public sealed class EndStepButtonController : AUIWindowController<EndStepButtonView>
    {
        private readonly IGamePhaseService _gamePhase;
        
        private Tween _tween;

        
        public EndStepButtonController(EndStepButtonView view, IGamePhaseService gamePhase) : base(view)
        {
            _gamePhase = gamePhase;
        }

        protected override void subscribe()
        {
            view.ButtonEndStep.Clicked += _endPlayerStep;

            _gamePhase.PhaseChanged += _onGamePhaseChanged;
        }

        protected override void unSubscribe()
        {
            view.ButtonEndStep.Clicked -= _endPlayerStep;
        }

        private void _endPlayerStep()
        {
            _gamePhase.RequestEndPlayerTurn();    
        }

        private void _onGamePhaseChanged(EGamePhase phase)
        {
            _tween?.Kill();
            
            _tween = view.ImageBackground.DOColor(phase is EGamePhase.PlayerTurn ? Color.white : Color.gray, 0.3f);
        }
    }
}