using Code.UI.Components;
using Code.UI.Windows.Base;
using UnityEngine;

namespace Code.UI.Windows.Game.Pause
{
    public class PauseWindowView : UIWindowView
    {
        [field: SerializeField] public UISlider SliderMusic { get; private set; }
        [field: SerializeField] public UISlider SliderSFX { get; private set; }
        [field: SerializeField] public UIDropDown LanguageDropDown { get; private set; }
    }
}
