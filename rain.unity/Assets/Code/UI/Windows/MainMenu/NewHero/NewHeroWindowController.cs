using System;
using Code.UI.Windows.Base;
using Code.UI.Windows.MainMenu.Delete;
using Core.Save;
using Core.Save.Models;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using Essential;

namespace Code.UI.Windows.MainMenu.NewHero
{
    public class NewHeroWindowController : UIWindowController<NewHeroWindowView>
    {
        public bool IsInitialized { get; set; }
        public event Action HeroCreated;
        
        private GameModel _gameModel;
        private DeleteWindowController _deleteWindow;
        private GamesSaver _gameSaver;


        public override UniTask InitializeWindow(UIWindowManager manager)
        {
            _gameSaver = Container.Instance.GetService<GamesSaver>();
            _gameModel = Container.Instance.GetService<GamesSaver>().Current;
            _deleteWindow = manager.GetWindow<DeleteWindowController>();

            Log.Info(this, $"Initialize window. game model is null = {_gameModel == null}");
            return base.InitializeWindow(manager);
        }

        public override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                view.InputFieldHeroName.Changed += _onInputFieldChanged;
                view.ButtonCreate.Clicked += _onButtonCreateClicked;
            }
            else
            {
                view.InputFieldHeroName.Changed -= _onInputFieldChanged;
                view.ButtonCreate.Clicked -= _onButtonCreateClicked;
            }
        }

        private void _onButtonCreateClicked()
        {
            _gameModel.Heroes.Add(new HeroModel
            {
                Name = view.InputFieldHeroName.Value,
                Stats = new CharacterStats
                {
                    Health = 100,
                    Armor = 0,
                    Agility = 0,
                    Strength = 0,
                    Endurance = 0,
                    Intellect = 0
                },
                GameTime = default,
                ExitTime = default
            });
            
            _gameSaver.Save();
            
            HeroCreated?.Invoke();
        }

        private void _onInputFieldChanged(string value)
        {
            view.ButtonCreate.SetInteractable(!string.IsNullOrEmpty(value));   
        }
    }
}