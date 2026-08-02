using Code.UI.Components;
using Essential;
using Essential.Data;
using UnityEngine;

namespace Code.UI
{
    public class UIRadioButton : UIButton, IPoolEntity
    {
        [field: SerializeField] public int Index { get; private set; }
        [field: SerializeField] public ReactiveProperty<bool> IsChecked { get; private set; }

        [SerializeField] private bool _isAutonomous;
        

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
            
            IsChecked.Value = true;
        }

        public void UnCheck()
        {
            if (!IsChecked.Value)
            {
                return;
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
