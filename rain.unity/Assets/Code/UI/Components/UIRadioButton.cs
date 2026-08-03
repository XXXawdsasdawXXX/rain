using Code.UI.Components.Animations;
using Essential;
using Essential.Data;
using UnityEngine;

namespace Code.UI.Components
{
    public class UIRadioButton : UIButton, IPoolEntity
    {
        [field: SerializeField] public int Index { get; private set; }
        [field: SerializeField] public ReactiveProperty<bool> IsChecked { get; private set; }

        [SerializeField] private bool _isAutonomous;

        [SerializeReference] protected AUIRadioAnimation[] radioAnimations;

        
        private void OnEnable()
        {
            if (IsChecked.Value)
            {
                Check();
            }
            else
            {
                UnCheck();
            }
        }

        public void Enable()
        {
            body.SetActive(true);
        }

        public void Disable()
        {
            body.SetActive(false);
        }
        
        public void SetValueWithoutNotify(bool value)
        {
            IsChecked.SetValueWithoutNotify(value);
        }

        public void SetIndex(int i)
        {
            Index = i;
        }

        public void Check()
        {
            if (IsChecked.Value)
            {
                return;
            }

            foreach (AUIRadioAnimation animation in radioAnimations)
            {
                animation.InvokeCheckAnimation();
            }
            
            IsChecked.Value = true;
        }

        public void UnCheck()
        {
            if (!IsChecked.Value)
            {
                return;
            }
            
            foreach (AUIRadioAnimation animation in radioAnimations)
            {
                animation.InvokeUnCheckAnimation();
            }

            IsChecked.Value = false;
        }

        public void ClearSubscriptions()
        {
            IsChecked.ClearSubscription();
        }

        protected override void onClick()
        {
            base.onClick();

            if (!_isAutonomous)
            {
                return;
            }

            if (IsChecked.Value)
            {
                UnCheck();
            }
            else
            {
                Check();
            }
        }
    }
}
