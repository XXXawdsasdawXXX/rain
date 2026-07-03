using Code.UI.Components;
using Code.UI.Components.Buttons;
using Code.UI.Windows.Base;
using UnityEngine;

namespace Code.UI.Windows.MainMenu.NewWorld
{
    public class NewWorldWindowView : UIWindowView
    {
        [field: SerializeField] public UIButton ButtonCreate { get; private set; }
        [field: SerializeField] public UIInputField InputFieldGameName { get; private set; }
    }
}