using Code.CoreGame.Entities.Characters.Controllers;
using Code.CoreGame.Entities.Characters.Hero.Components;
using Code.CoreGame.Harvest.Data;
using Core.Common.Extensions;
using Core.GameLoop;
using Core.Save;
using Core.Save.Models;
using Core.ServiceLocator;
using Core.ServiceLocator.MarkerInterfaces;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace Code.CoreGame.Harvest
{
    public class ResourcesManager : Essential.Mono, IService, IInitializeListener, ISubscriber
    {
        public bool IsInitialized { get; set; }

        [SerializeField] private ResourceCollection _resourceCollection;
        [SerializeField] private Resource[] _resources;

        private Hero _hero;
        private Mainer _mainer;
        private GameModel _gameModel;

        
        public UniTask Initialize()
        {
            _gameModel = Container.Instance.GetService<GamesSaver>().Current;

           // _hero = _userProvider.GetHeroComponent<Hero>(); todo set hero value
            _mainer = _hero.GetCharacterComponent<Mainer>();
            return UniTask.CompletedTask;
        }

        /* todo load game resources
        public UniTask GameLoad(GameModel model)
        {
            foreach (Resource resource in _resources)
            {
                string resourcePosition = resource.Position.AsString();
                if (model.World.SceneResources.ContainsKey(resourcePosition))
                {
                    resource.SetValue(model.World.SceneResources[resourcePosition]);
                }
                else
                {
                    model.World.SceneResources.Add(resourcePosition, resource.CurrentValue);
                }
            }
            
            return UniTask.CompletedTask;
        }
        */
        
        public void Subscribe()
        {
            foreach (Resource resource in _resources)
            {
                resource.HarvestStarted += _onHarvestStarted;
                resource.Changed += _onChangedResource;
            }
        }

        public void Unsubscribe()
        {
            foreach (Resource resource in _resources)
            {
                resource.HarvestStarted -= _onHarvestStarted;
                resource.Changed -= _onChangedResource;
            }
        }

        private void _onChangedResource(Resource resource)
        {
            _gameModel.World.SceneResources[resource.Position.AsString()] = resource.CurrentValue;
        }

        private void _onHeroCreated()
        {
           
        }

        private void _onHarvestStarted(Resource resource)
        {
            if (!_mainer.IsMining)
            {
                _mainer.StartHarvest(resource);
            }
        }

#if UNITY_EDITOR

        [Button]
        private void _validateResources()
        {
            foreach (Resource resource in _resources)
            {
                resource.Validate(_resourceCollection.Library.Get(resource.Type));
                EditorUtility.SetDirty(resource);
            }

            AssetDatabase.SaveAssets();
        }

        [Button]
        private void _findResources()
        {
            _resources = GetComponentsInChildren<Resource>(true);
        }
#endif
 
    }
}