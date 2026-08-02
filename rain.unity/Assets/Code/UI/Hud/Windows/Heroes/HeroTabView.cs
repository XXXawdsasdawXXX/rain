using Code.Game.Save;
using Code.UI.Components;
using Essential;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Hud.Windows
{
    public class HeroTabView : UIRadioButton
    {
        public CharacterModel Model { get; private set; }
        
        [SerializeField] private UIText _textHeroName;
        [SerializeField] private Image _renderBackground;

        
        public void SetCharacterModel(CharacterModel model)
        {
            Model = model;
            _textHeroName.SetText(model.Name);
        }

     

        /*public void Select()
        {
            _renderBackground.color = UIConstance.SelectHeroColor;
        }
        
        public void Deselect()
        {
            _renderBackground.color = UIConstance.DefaultHeroColor;
        }*/
    }
}