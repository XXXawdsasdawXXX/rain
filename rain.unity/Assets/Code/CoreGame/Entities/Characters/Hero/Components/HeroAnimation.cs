using Code.CoreGame.Entities.Animation;
using Code.CoreGame.Entities.Characters.Interfaces;
using Core.Common.Data;
using Core.GameLoop;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;

namespace Code.CoreGame.Entities.Characters.Hero.Components
{
    public class HeroAnimation : Essential.Mono, IHarvestAnimator, IAnimationStateReader,
    IInitializeListener, IUpdateListener
    {
        public bool IsInitialized { get; set; }
        [ShowInInspector] public AnimatorKey.ECharacterAnimationState CurrentState { get; private set; }
        
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _viewBody;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private CharacterSkeletonAnimator _skeletonAnimator;

        private Cache<Vector3> _velocityCache;

        public UniTask Initialize()
        {
            _velocityCache = new Cache<Vector3>();
            _ensureSkeletonAnimator();
            
            return UniTask.CompletedTask;
        }

        public void GameUpdate(float deltaTime)
        {
            if (_velocityCache.Update(_rigidbody2D.velocity))
            {
                _animator.SetFloat(AnimatorKey.PARAM_SPEED, _rigidbody2D.velocity.magnitude);
       
                if (_rigidbody2D.velocity.x != 0)
                {
                    _rotateServerRpc(_rigidbody2D.velocity.x);
                }
            }
        }

        
        [Button] public void StartEat()
        {
            _animator.SetBool(AnimatorKey.PARAM_EAT, true);
        }

        [Button
        ] public void StopEat()
        {
            _animator.SetBool(AnimatorKey.PARAM_EAT, false);
        }

        [Button] public void StartMine(AnimatorKey.EHarvestType harvestType)
        {
            int type = (int)harvestType;
            _animator.SetInteger(AnimatorKey.PARAM_HARVEST_TYPE, type);
        }

        [Button] public void StopMine()
        {
            _animator.SetInteger(AnimatorKey.PARAM_HARVEST_TYPE, 0);
        }


        private void _rotateServerRpc(float velocityX)
        {
            float forward = velocityX < 0 ? -1 : 1;
            _skeletonAnimator.SetFacing(forward > 0);
        }
        

        public void EnteredState(int stateHash)
        {
            if (AnimatorKey.CHARACTER_STATES.ContainsKey(stateHash))
            {
                CurrentState = AnimatorKey.CHARACTER_STATES[stateHash];
            }

            _skeletonAnimator?.EnteredState(stateHash);
        }

        public void ExitedState(int stateHash)
        {
            if (AnimatorKey.CHARACTER_STATES.ContainsKey(stateHash))
            {
                CurrentState = AnimatorKey.CHARACTER_STATES[stateHash];
            }

            _skeletonAnimator?.ExitedState(stateHash);
        }

        private void _ensureSkeletonAnimator()
        {
            if (_skeletonAnimator == null)
            {
                _skeletonAnimator = GetComponent<CharacterSkeletonAnimator>();
            }

            if (_skeletonAnimator == null)
            {
                _skeletonAnimator = gameObject.AddComponent<CharacterSkeletonAnimator>();
            }

            _skeletonAnimator.Bind(_animator, _viewBody);
        }
    }
}
