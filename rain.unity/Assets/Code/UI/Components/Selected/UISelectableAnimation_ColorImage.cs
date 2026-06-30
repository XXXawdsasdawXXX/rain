using Code.UI.Data;
using Core.GameLoop;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.UI.Components.Selected
{
    public class UISelectableAnimation_ColorImage : UISelectableAnimation, IInitializeListener
    {
        public bool IsInitialized { get; set; }

        [SerializeField] private UIImage _uiImage;

        private UISettings _uiSettings;

        public UniTask Initialize()
        {
            _uiSettings = Container.Instance.GetSO<UISettings>();
      
            return UniTask.CompletedTask;
        }

        public override void Select()
        {
            _uiImage.Colorize(_uiSettings.SelectedTween);
        }

        public override void Deselect()
        {
            _uiImage.Colorize(_uiSettings.DeselectedTween);
        }
    }
}