using Code.UI.Data;
using Core.GameLoop;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.UI.Components.Selected
{
    public class UISelectableAnimation_ColorText : UISelectableAnimation, IInitializeListener
    {
        public bool IsInitialized { get; set; }

        [SerializeField] private UIText _uiText;

        private UISettings _uiSettings;

        public UniTask Initialize()
        {
            _uiSettings = Container.Instance.GetSO<UISettings>();

            return UniTask.CompletedTask;
        }

        public override void Select()
        {
            _uiText.Colorize(_uiSettings.SelectedTween);
        }

        public override void Deselect()
        {
            _uiText.Colorize(_uiSettings.DeselectedTween);
        }

        private void OnValidate()
        {
            if (_uiText == null)
            {
                TryGetComponent(out _uiText);
            }
        }
    }
}