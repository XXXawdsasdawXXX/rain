using Code.Game.Characters.Armor;
using Code.Game.Save;
using Code.UI.Components;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Hud.Windows.Heroes
{
    public class HeroTabView : UIRadioButton
    {
        public CharacterModel Model { get; private set; }
        
        [SerializeField] private UIText _textHeroName;
        [SerializeField] private Image _renderBackground;
        
        [Space, Title("Avatar")]
        [SerializeField] private Image _imageArmorHead;
        [SerializeField] private Image _imageArmorMask;
        [SerializeField] private Image _imageArmorTorso;
        
        private ArmoryLibrary _armoryLibrary;


        public void Construct(ArmoryLibrary armoryLibrary)
        {
            _armoryLibrary = armoryLibrary;
        }

        public void SetCharacterModel(CharacterModel model)
        {
            if (Model != null)
            {
                Model.ArmorChanged -= _updateArmorPiece;
            }
            
            Model = model;
            Model.ArmorChanged += _updateArmorPiece;
            
            foreach ((EBodyPart part, int setIndex) in Model.Armor)
            {
                _updateArmorPiece(part, setIndex);
            }
            
            _textHeroName.SetText(Model.Name);
        }

        private void _updateArmorPiece(EBodyPart bodyPart, int setIndex)
        {
            Image render;
            switch (bodyPart)
            {
                case EBodyPart.Mask:
                   render = _imageArmorMask;
                   break;
                case EBodyPart.Head:
                   render = _imageArmorHead;
                    break;
                case EBodyPart.Torso:
                    render = _imageArmorTorso;
                    break;
                case EBodyPart.Pants:
                case EBodyPart.Shoes:
                default:
                    return;
            }

            ArmorSetModel setModel = _armoryLibrary.Armory.Get(setIndex);
            
            Sprite[] armorSprites = setModel.Pieces[(int)bodyPart].Views;
            Sprite armorPiece = armorSprites.Length > 0 ? armorSprites[0] : null;
            Color color = armorPiece == null ? Color.clear : Color.white;

            render.sprite = armorPiece;
            render.color = color;
        }
    }
}   