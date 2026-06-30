using Code.UI.Components;
using Code.UI.Components.Buttons;
using Code.UI.Windows.Base;
using UnityEngine;

namespace Code.UI.Windows.MainMenu.NewHero
{
    public class NewHeroWindowView : UIWindowView
    { 
        [field: SerializeField] public UIInputField InputFieldHeroName { get; private set; }
        [field: SerializeField] public  UIButton ButtonCreate { get; private set; }
    }
}