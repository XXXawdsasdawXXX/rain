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
    public sealed class CharacterFactory : ICharacterFactory
    {
        private readonly IObjectResolver _resolver;
        private readonly AssetLibrary _assetLibrary;
        private readonly IPlayerPartyProvider _playerPartyProvider;
        private readonly GameSaveRepository _gameSaveRepository;


        public CharacterFactory(
            IObjectResolver resolver, 
            AssetLibrary assetLibrary, 
            IPlayerPartyProvider playerPartyProvider, 
            GameSaveRepository gameSaveRepository)
        {
            _resolver = resolver;
            _assetLibrary = assetLibrary;
            _playerPartyProvider = playerPartyProvider;
            _gameSaveRepository = gameSaveRepository;
        }

        public CharacterView SpawnHero(CharacterModel model)
        {
            CharacterView heroPrefab = _assetLibrary.SceneComponents
                .Get(AssetsKey.CHARACTER)
                .GetComponent<CharacterView>();
            
            CharacterView heroInstance = _resolver.Instantiate(
                heroPrefab, 
                model.Position.AsVector3(), 
                Quaternion.identity);
            
            heroInstance.SetModel(model);
            
            _playerPartyProvider.AddHero(heroInstance);

            GameModel gameSave = _gameSaveRepository.GetCurrentGame();

            if (!gameSave.Heroes.Exists(h => h.ID.Equals(model.ID)))
            { 
                gameSave.Heroes.Add(model);
            }
            
            return heroInstance;
        }

        public CharacterView SpawnNpc(CharacterModel model)
        {
            CharacterView npcPrefab = _assetLibrary.SceneComponents
                .Get(AssetsKey.CHARACTER)
                .GetComponent<CharacterView>();
            
            CharacterView npcInstance = _resolver.Instantiate(
                npcPrefab, 
                model.Position.AsVector3(), 
                Quaternion.identity);
            
            npcInstance.SetModel(model);
            
            GameModel gameSave = _gameSaveRepository.GetCurrentGame();

            if (!gameSave.Npc.Exists(h => h.ID.Equals(model.ID)))
            { 
                gameSave.Npc.Add(model);
            }

            return npcInstance;
        }
    }
}