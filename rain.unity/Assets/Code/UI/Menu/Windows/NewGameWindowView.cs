using Code.Game.Save;
using Code.UI.Components;
using Code.UI.Components.Window;
using UnityEngine;

namespace Code.UI.Menu
{
    public sealed class NewGameWindowView : AUIWindowView
    {
        [field: SerializeField] public UIInputField InputFiled_HeroName { get; private set; }
        [field: SerializeField] public UIButton Button_Create { get; private set; }
    }

    public sealed class NewGameWindowController : AUIWindowController<NewGameWindowView>
    {
        private readonly GameSaveRepository _gameSave;

        public NewGameWindowController(NewGameWindowView view,
            GameSaveRepository gameSave) : base(view)
        {
            _gameSave = gameSave;
        }

        protected override void subscribe()
        {
            view.Button_Create.Clicked += _onClickedCreate;
        }

        protected override void unSubscribe()
        {
            view.Button_Create.Clicked -= _onClickedCreate;
        }

        private void _onClickedCreate()
        {
            _gameSave.AddNewGame(new HeroModel(view.InputFiled_HeroName.Value), new WorldModel());
        }
    }
}