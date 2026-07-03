using Code.UI.Components;
using Code.UI.Components.Buttons;
using Code.UI.Windows.Base;
using UnityEngine;
using UnityEngine.Localization;

namespace Code.UI.Windows.MainMenu.Game
{
    public class WorldWindowView : UIWindowView
    {
        [field: SerializeField] public GameObject ObjectLocker { get; private set; }
        [field: SerializeField] public UIButton ButtonNew { get; private set; }
        [field: SerializeField] public UIButton ButtonContinue { get; private set; }
        [field: SerializeField] public UIButton ButtonDelete { get; private set; }
        [field: SerializeField] public UIRadioGroup<UIText> WorldsRadioGroup { get; private set; }
        [field: SerializeField] public UIScroll Scroll { get; private set; }
    }
}
