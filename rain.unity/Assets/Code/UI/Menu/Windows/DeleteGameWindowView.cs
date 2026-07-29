using Code.Game.Save;
using Code.UI.Components;
using Code.UI.Components.Window;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.UI.Menu
{
    public sealed class DeleteGameWindowView : AUIWindowView
    {
        public struct Model
        {
            public string ContextHeroName;
        }

        [field: SerializeField] public Model CurrenModel { get; private set; }
        [field: SerializeField] public UIText Text_Description { get; private set; }
        [field: SerializeField] public UIButton Button_Delete { get; private set; }
        
        public void SetNewModel(Model model)
        {
            CurrenModel = model;
            Text_Description.SetLocalizationArguments(CurrenModel.ContextHeroName);
        }
    }

    [UsedImplicitly]
    public sealed class DeleteGameWindowController : AUIWindowController<DeleteGameWindowView>
    {
        private readonly GameSaveRepository _gameSave;
        
        public DeleteGameWindowController(DeleteGameWindowView view, GameSaveRepository gameSave) : base(view)
        {
            _gameSave = gameSave;
        }

        public override void Start()
        {
            base.Start();
            
            _updateContext();
        }

        protected override void subscribe()
        {
            _gameSave.Changed += _updateContext;
            
            view.Button_Delete.Clicked += _onClickedDelete;
        }

        protected override void unSubscribe()
        {
            _gameSave.Changed -= _updateContext;
            
            view.Button_Delete.Clicked -= _onClickedDelete;
        }

        private void _onClickedDelete()
        {
            view.Close();
          
            _gameSave.DeleteLastGame();
        }

        private void _updateContext()
        {
            if (_gameSave.TryGetLastGame(out GameModel game))
            {
                view.SetNewModel(new DeleteGameWindowView.Model
                {
                    ContextHeroName = game.Heroes[0].Name
                });
            }
        }
    }
}