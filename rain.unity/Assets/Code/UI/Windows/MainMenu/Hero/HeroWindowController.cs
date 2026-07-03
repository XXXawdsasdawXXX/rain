using System;
using Code.UI.Components;
using Code.UI.Components.GameEntities;
using Code.UI.Windows.Base;
using Code.UI.Windows.MainMenu.Delete;
using Code.UI.Windows.MainMenu.NewHero;
using Core.Save;
using Core.Save.Models;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;

namespace Code.UI.Windows.MainMenu.Hero
{
    public class HeroWindowController : UIWindowController<HeroWindowView>
    {
        public event Action HeroListChanged;

        private NewHeroWindowController _newHeroWindow;
        private DeleteWindowController _deleteWindow;

        private GamesSaver _gameSaver;


        public override UniTask InitializeWindow(UIWindowManager manager)
        {
            _gameSaver = Container.Instance.GetService<GamesSaver>();
            
            _newHeroWindow = manager.GetWindow<NewHeroWindowController>();
            _deleteWindow = manager.GetWindow<DeleteWindowController>();

            view.HeroesRadioGroup.Initialize();

            return base.InitializeWindow(manager);
        }

        public override void LoadWindow(GameModel model)
        {
            _updateHeroesList();

            _updateOptionButtonsView();

            _updateSelectedHeroView(model.LastHeroIndex.Value);
        }

        public override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                view.ButtonNew.Clicked += _openNewHeroWindow;
                view.ButtonDelete.Clicked += _openDeleteHeroWindow;
               
                view.HeroesRadioGroup.Selected += _updateSelectedHeroView;
                view.HeroesRadioGroup.Selected += _updateSelectedHeroValue;

                _newHeroWindow.HeroCreated += _updateHeroesList;
            }
            else
            {
                view.ButtonNew.Clicked -= _openNewHeroWindow;
                view.ButtonDelete.Clicked -= _openDeleteHeroWindow;

                view.HeroesRadioGroup.Selected -= _updateSelectedHeroView;
                view.HeroesRadioGroup.Selected -= _updateSelectedHeroValue;

                _newHeroWindow.HeroCreated -= _updateHeroesList;
            }
        }
        
        private void _openNewHeroWindow()
        {
            windowManager.SwitchWindow<NewHeroWindowController>();
        }
        
        private void _openDeleteHeroWindow()
        {
            windowManager.SwitchWindow<DeleteWindowController>();

            GameModel model = _gameSaver.Current;
            
            _deleteWindow.SetObserved(model.Hero.Name, () =>
            {
                model.Heroes.RemoveAt(model.LastHeroIndex.Value);

                int lastHeroIndex = model.Heroes.IndexOf(model.GetNearestHeroByExitTime());
                model.LastHeroIndex.Value = lastHeroIndex >= 0 ? lastHeroIndex : 0;
                
                _updateHeroesList();
                _updateOptionButtonsView();
            });
        }

        private void _updateHeroesList()
        {
            view.HeroesRadioGroup.Clear();
            
            GameModel model = _gameSaver.Current;

            if (model.Heroes != null && model.Heroes.Count > 0)
            {
                for (int i = 0; i < model.Heroes.Count; i++)
                {
                    HeroModel heroModel = model.Heroes[i];
                    UIText text = view.HeroesRadioGroup.Pool.GetNext();
                    text.SetIndex(i);
                    text.SetText(heroModel.Name);
                    text.Deselect();
                }

                view.HeroesRadioGroup.Pool.SortByIndex();
            }
            
            _updateSelectedHeroView(model.LastHeroIndex.Value);

            HeroListChanged?.Invoke();
        }

        private void _updateSelectedHeroValue(int heroIndex)
        {
            GameModel model = _gameSaver.Current;
            
            if (model.Heroes.Count > heroIndex && heroIndex >= 0)
            {
                model.LastHeroIndex.Value = heroIndex;
            }
        }
        
        private void _updateSelectedHeroView(int heroIndex)
        {
            GameModel model = _gameSaver.Current;
            
            if (model.Heroes.Count > heroIndex && heroIndex >= 0)
            {
                view.BodyHeroView.SetActive(true);
                view.HeroCard.SetModel(new UIHeroCardView.Model(
                    model.Heroes[heroIndex].Name,
                    model.Heroes[heroIndex].GameTime.ToString()));
                view.HeroesRadioGroup.Select(heroIndex);
            }
            else
            {
                view.BodyHeroView.SetActive(false);
            }
        }

        private void _updateOptionButtonsView()
        {
            GameModel model = _gameSaver.Current;
            bool isHasHero = model.Heroes.Count >= 0;
            view.ButtonDelete.SetInteractable(isHasHero);
        }
    }
}