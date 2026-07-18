using Code.Game.Characters;
using Code.Game.Factories;
using Code.Game.Save;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Code.Game.StateMachine
{
    [UsedImplicitly]
    public sealed class GameWorldBootstrap : IStartable
    {
        private readonly GameSaveRepository _gameSave;
        private readonly IHeroFactory _heroFactory;
        private readonly IHeroProvider _heroProvider;

        
        public GameWorldBootstrap(GameSaveRepository gameSave, IHeroFactory heroFactory, IHeroProvider heroProvider)
        {
            _gameSave = gameSave;
            _heroFactory = heroFactory;
            _heroProvider = heroProvider;
        }
        
        public void Start()
        {
            _createHero();
        }

        private void _createHero()
        {
            CharacterModel heroModel = _gameSave.GetCurrentGame().hero;
            CharacterView heroInstance = _heroFactory.SpawnHero(heroModel);
            _heroProvider.Set(heroInstance);
        }
    }
}