using Code.Game.Characters;
using Code.Game.Characters.Hero;
using Code.Game.Save;
using Core.AssetManagement.Libraries.Assets;
using Essential.Extensions;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Game.Factories
{
    [UsedImplicitly]
    public sealed class HeroFactory : IHeroFactory
    {
        private readonly IObjectResolver _resolver;
        private readonly AssetLibrary _assetLibrary;
        private readonly IPlayerPartyProvider _playerPartyProvider;


        public HeroFactory(IObjectResolver resolver, AssetLibrary assetLibrary, IPlayerPartyProvider playerPartyProvider)
        {
            _resolver = resolver;
            _assetLibrary = assetLibrary;
            _playerPartyProvider = playerPartyProvider;
        }

        public CharacterView SpawnHero(CharacterModel model)
        {
            CharacterView heroPrefab = _assetLibrary.SceneComponents.Get(AssetsKey.CHARACTER).GetComponent<CharacterView>();
            
            CharacterView heroInstance = _resolver.Instantiate(
                heroPrefab, 
                model.Position.AsVector3(), 
                Quaternion.identity);
            
            heroInstance.SetModel(model);
            
            _playerPartyProvider.AddCharacter(heroInstance);
            
            return heroInstance;
        }
    }
}