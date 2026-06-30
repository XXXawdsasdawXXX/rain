using Code.UI.Components;
using Code.UI.Components.Buttons;
using Code.UI.Windows.Base;
using UnityEngine;

namespace Code.UI.Windows.MainMenu.NewGame
{
    public class NewGameWindowView : UIWindowView
    {
        [field: SerializeField] public UIButton ButtonCreate { get; private set; }
        [field: SerializeField] public UIInputField InputFieldGameName { get; private set; }
    }
}