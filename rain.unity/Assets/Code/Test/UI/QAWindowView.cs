using Code.Game.Factories;
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
        [field: SerializeField] public UIText TextGamePhase { get; private set; }
        [field: SerializeField] public UIButton ButtonOpenWindow { get; private set; }
        [field: SerializeField] public UIButton ButtonCloseWindow { get; private set; }
        [field: SerializeField] public UIButton ButtonSave { get; private set; }
        [field: SerializeField] public UIButton ButtonMainMenu { get; private set; }
        [field: SerializeField] public UIButton ButtonAddHero { get; private set; }
    }

    
    [UsedImplicitly]
    public class QAWindowController : AUIWindowController<QAWindowView>
    {
        private readonly GameSaveRepository _gameSave;
        private readonly GameStateMachine _gameStateMachine;
        private readonly ICharacterFactory _characterFactory;
        private readonly IGamePhaseService _gamePhase;


        public QAWindowController(QAWindowView view, 
            GameSaveRepository gameSave, 
            GameStateMachine gameStateMachine,
            ICharacterFactory characterFactory,
            IGamePhaseService gamePhase) : base(view)
        {
            _gameSave = gameSave;
            _gameStateMachine = gameStateMachine;
            _characterFactory = characterFactory;
            _gamePhase = gamePhase;
        }

        public override void Start()
        {
            base.Start();
            
            _updateGamePhaseText(_gamePhase.Phase);
        }

        protected override void subscribe()
        {
            _gamePhase.PhaseChanged += _updateGamePhaseText;
            
            view.ButtonOpenWindow.Clicked += view.Open;
            view.ButtonCloseWindow.Clicked += view.Close;

            view.ButtonAddHero.Clicked += _addHero;
            view.ButtonSave.Clicked += _saveGame;
            view.ButtonMainMenu.Clicked += _loadMainMenu;
        }

        protected override void unSubscribe()
        {
            _gamePhase.PhaseChanged -= _updateGamePhaseText;
            
            view.ButtonOpenWindow.Clicked -= view.Open;
            view.ButtonCloseWindow.Clicked -= view.Close;
            
            view.ButtonAddHero.Clicked -= _addHero;
            view.ButtonSave.Clicked -= _saveGame;
            view.ButtonMainMenu.Clicked -= _loadMainMenu;
        }

        private void _updateGamePhaseText(EGamePhase phase)
        {
            view.TextGamePhase.SetText(phase.ToString());
        }

        private void _addHero()
        {
            _characterFactory.SpawnHero(new CharacterModel($"{Random.Range(0, 10)},{Random.Range(0, 10)},{Random.Range(0, 10)}"));
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