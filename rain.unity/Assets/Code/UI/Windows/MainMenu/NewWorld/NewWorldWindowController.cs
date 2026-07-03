using System;
using System.Collections.Generic;
using System.Linq;
using Code.UI.Windows.Base;
using Core.Save;
using Core.Save.Models;
using Core.ServiceLocator;
using Core.Time;
using Cysharp.Threading.Tasks;

namespace Code.UI.Windows.MainMenu.NewWorld
{
    public class NewWorldWindowController : UIWindowController<NewWorldWindowView>
    {
        public event Action ModelAdded; 
        
        private GameModel _gameModel;
        private WorldTime _worldTime;
        private GamesSaver _gameSaver;

        public override UniTask InitializeWindow(UIWindowManager manager)
        {
            _gameSaver = Container.Instance.GetService<GamesSaver>();
            _gameModel = Container.Instance.GetService<GamesSaver>().Current;
            _worldTime = Container.Instance.GetService<WorldTime>();

            return base.InitializeWindow(manager);
        }

        public override void StartWindow()
        {
            _updateCreateButtonState(view.InputFieldGameName.Value);
        }

        public override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                view.ButtonCreate.Clicked += _addWorld;
                view.InputFieldGameName.Changed += _updateCreateButtonState;
            }
            else
            {
                view.ButtonCreate.Clicked -= _addWorld;
                view.InputFieldGameName.Changed -= _updateCreateButtonState;
            }
        }

        private void _updateCreateButtonState(string value)
        {
            view.ButtonCreate.SetInteractable(!string.IsNullOrEmpty(value));
        }

        private void _addWorld()
        {
            bool containsName = _gameModel.Worlds.Any(world => world.Name.Equals(view.InputFieldGameName.Value));

            if (containsName)
            {
                return;
            }
            
            _gameModel.Worlds.Add(new WorldModel
            {
                Name = view.InputFieldGameName.Value,
                CreateTime = _worldTime.Current,
                ExitTime =  _worldTime.Current,
                SceneResources = new Dictionary<string, int>(),
            });

            _gameModel.LastWorldIndex.Value = _gameModel.Worlds.Count - 1;
            
            _gameSaver.Save();
            
            ModelAdded?.Invoke();
        }
    }
}