using Code.CoreGame.Entities.Characters.Hero.Components;
using Code.CoreGame.Entities.Characters.Interfaces;
using Code.CoreGame.Harvest;
using Core.Common.Data;
using Core.GameLoop;
using Core.ServiceLocator;

namespace Code.CoreGame.Entities.Characters.Controllers
{
    public class Mainer : IUpdateListener, ICharacterComponent
    {
        public Condition Condition { get; } = new();
        public bool IsMining { get; private set; }
        private MinerProcess _process { get; set; }

        private readonly ResourceStorage _resourceStorage;
        
        private readonly IHarvestAnimator _animator;
        private readonly CurrentMaxValue _health;

        
        public Mainer(IHarvestAnimator animator, CurrentMaxValue health)
        {
            _resourceStorage = Container.Instance.GetService<ResourceStorage>();
            
            _animator = animator;
            _health = health;
        }

        public void GameUpdate(float deltaTime)
        {
            if (!IsMining || !Condition.AreMet())
            {
                if (IsMining)
                {
                    _stopHarvest();
                }

                return;
            }

            _process.CurrentTime += deltaTime;

            if (_process.CurrentTime >= _process.Resource.Config.HarvestTime)
            {
                _health.Decrease(_process.Resource.Config.HealthPrice);

                _process.Resource.UpdateValue(-_process.Resource.Config.ResourcePerTick);

                _resourceStorage.Add(_process.Resource.Type, _process.Resource.Config.ResourcePerTick);
                
                if (_process.Resource.IsEnded)
                {
                    _stopHarvest();
                }
                else
                {
                    _process.CurrentTime = 0;
                }
            }
        }

        public void StartHarvest(Resource resource)
        {
            _process = new MinerProcess
            {
                Resource = resource,
                CurrentTime = 0,
            };

            _animator.StartMine(resource.Config.HarvestType);
            
            IsMining = true;
        }

        private void _stopHarvest()
        {
            _process = null;

            _animator.StopMine();

            IsMining = false;
        }
    }

    public class MinerProcess
    {
        public Resource Resource;
        public float CurrentTime;
    }
}