using Code.UI.Components.Selected;
using Core.Audio.AudioSystem;
using Core.GameLoop;
using Core.Libraries.Audio;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.UI.Components.Buttons
{
    public class UIButton : UISelectable, IPointerUpHandler, IInitializeListener
    {
        public bool IsInitialized { get; set; }

        [SerializeField] private Button _button;
        
        private AudioService _audio;

        public UniTask Initialize()
        {
            _audio = Container.Instance.GetService<AudioService>();
            
            return UniTask.CompletedTask;
        }

        public override void SetInteractable(bool isInteractable)
        {
            _button.interactable = isInteractable;
        }

        protected override void onClick()
        {
            _audio?.OneShot(AudioEventLibrary.BUTTON_DOWN);   
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _audio?.OneShot(AudioEventLibrary.BUTTON_UP);
        }
    }
}