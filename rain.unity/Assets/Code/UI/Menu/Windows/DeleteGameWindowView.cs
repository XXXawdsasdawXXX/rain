using Code.Game.Save;
using Code.UI.Components;
using Code.UI.Components.Window;
using UnityEngine;

namespace Code.UI.Menu
{
    public sealed class DeleteGameWindowView : AUIWindowView
    {
        public struct Model
        {
            public int ContextGameIndex;
        }

        [field: SerializeField] public Model CurrenModel { get; private set; }
        [field: SerializeField] public UIText Text_Description { get; private set; }
        [field: SerializeField] public UIButton Button_Delete { get; private set; }
        
        public void SetNewModel(Model model)
        {
            CurrenModel = model;
            Text_Description.SetLocalizationArguments(CurrenModel.ContextGameIndex);
        }
    }

    public sealed class DeleteGameWindowController : AUIWindowController<DeleteGameWindowView>
    {
        private readonly GameSaveRepository _gameSave;


        public DeleteGameWindowController(DeleteGameWindowView view,
            GameSaveRepository gameSave) : base(view)
        {
            _gameSave = gameSave;
        }
        
        protected override void subscribe()
        {
            view.Button_Delete.Clicked += _onClickedDelete;
        }

        protected override void unSubscribe()
        {
            view.Button_Delete.Clicked -= _onClickedDelete;
        }

        private void _onClickedDelete()
        {
            _gameSave.DeleteGame(view.CurrenModel.ContextGameIndex);
        }
    }
}