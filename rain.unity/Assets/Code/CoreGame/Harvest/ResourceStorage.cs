using System;
using System.Collections.Generic;
using System.Linq;
using Code.CoreGame.Harvest.Data;
using Core.Save.Models;
using Core.ServiceLocator.MarkerInterfaces;
using Cysharp.Threading.Tasks;

namespace Code.CoreGame.Harvest
{
    
    //todo fix loading
    public class ResourceStorage : IService
    {
        public event Action<KeyValuePair<EResource, int>> ValueChanged;
        
        public event Action<Dictionary<EResource, int>> CollectionChanged;
        public Dictionary<EResource, int> Collection { get; private set; }
        
        private GameModel _model;
        
        
        public UniTask GameLoad(GameModel model)
        {
            _model = model;
            
            Dictionary<int, int> savedCollection = model.Hero.ResourcesStorage;

            Collection = new Dictionary<EResource, int>();

            EResource[] resourceTypes = Enum.GetValues(typeof(EResource)).Cast<EResource>().ToArray();
            
            foreach ((int resourceType, int amount) in savedCollection)
            {
                Collection.Add((EResource)resourceType, amount);
            }

            if (savedCollection.Count() < resourceTypes.Length - 1)
            {
                for (int i = 1; i < resourceTypes.Length; i++)
                {
                    if(!Collection.ContainsKey(resourceTypes[i]))
                    {
                        Collection.Add(resourceTypes[i], 0);
                    }
                }
            }
    
            CollectionChanged?.Invoke(Collection);
    
            return UniTask.CompletedTask;
        }
        
        public void Add(EResource resource, int value)
        {
            if (!Collection.ContainsKey(resource))
            {
                Collection.Add(resource, 0);
            }
            
            Collection[resource] += value;

            _model.Hero.ResourcesStorage[(int)resource] = Collection[resource];

            ValueChanged?.Invoke(new KeyValuePair<EResource, int>(resource, Collection[resource]));
        }

        public void Spend(EResource resource, int value)
        {
            if (!Collection.ContainsKey(resource))
            {
                Collection.Add(resource, 0);
            }
            
            Collection[resource] -= value;
            
            _model.Hero.ResourcesStorage[(int)resource] = Collection[resource];
            
            
            ValueChanged?.Invoke(new KeyValuePair<EResource, int>(resource, Collection[resource]));
        }
    }
}