using Code.Game.Characters;
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


        public HeroFactory(IObjectResolver resolver, AssetLibrary assetLibrary)
        {
            _resolver = resolver;
            _assetLibrary = assetLibrary;
        }
        
        public CharacterView SpawnHero(Vector3 position)
        {
            return null;
        }

        public CharacterView SpawnHero(CharacterModel model)
        {
            CharacterView heroPrefab = _assetLibrary.SceneComponents.Get(AssetsKey.CHARACTER).GetComponent<CharacterView>();
            CharacterView heroInstance = _resolver.Instantiate(
                heroPrefab, 
                model.Position.AsVector3(), 
                Quaternion.identity);
            
            return heroInstance;
        }
    }
}