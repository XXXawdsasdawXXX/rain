using System;
using System.Collections.Generic;
using System.Linq;
using Essential;
using JetBrains.Annotations;

namespace Code.Game.Characters.Hero
{
    [UsedImplicitly]
    public class PlayerPartyProvider : IPlayerPartyProvider
    {
        public event Action<CharacterView> CharacterAdded;
        public event Action<CharacterView> CharacterSelected;
        public List<CharacterView> Heroes { get; } = new();
        public List<CharacterView> SelectedCharacters { get; } = new List<CharacterView>();
        public CharacterView CurrentCharacter { get; }


        public void AddHero(CharacterView characterView)
        {
            if (Heroes.Exists(c => c.Model.ID == characterView.Model.ID))
            {
                Log.Error(this, "Character already exist into collection");
                return;
            }
            
            Heroes.Add(characterView);
            
            CharacterAdded?.Invoke(characterView);

            if (Heroes.Count == 1)
            {
                SetSelectedCharacter(characterView);
            }
        }

        public void SetSelectedCharacter(CharacterView selectCharacter)
        {
            if (SelectedCharacters.Contains(selectCharacter))
            {
                return;
            }
         
            SelectedCharacters.Clear();
            
            SelectedCharacters.Add(selectCharacter);
            
            CharacterSelected?.Invoke(selectCharacter);
        }

        public void ClearSelectedCharacter()
        {
            SelectedCharacters.Clear();
        }
    }
}