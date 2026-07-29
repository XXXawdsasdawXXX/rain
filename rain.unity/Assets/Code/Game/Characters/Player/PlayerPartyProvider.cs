using System;
using System.Collections.Generic;
using Essential;
using JetBrains.Annotations;

namespace Code.Game.Characters.Hero
{
    [UsedImplicitly]
    public class PlayerPartyProvider : IPlayerPartyProvider
    {
        public event Action<CharacterView> CharacterAdded;
        public event Action<CharacterView> CharacterSelected;
        public List<CharacterView> Characters { get; } = new();
        public List<CharacterView> SelectedCharacters { get; } = new List<CharacterView>();
        public CharacterView CurrentCharacter { get; }


        public void AddHero(CharacterView characterView)
        {
            if (Characters.Contains(characterView))
            {
                Log.Error(this, "Character already exist into collection");
                return;
            }
            
            Characters.Add(characterView);
            
            CharacterAdded?.Invoke(characterView);

            if (Characters.Count == 1)
            {
                AddSelectCharacter(characterView);
            }
        }

        public void AddSelectCharacter(CharacterView selectCharacter)
        {
            if (SelectedCharacters.Contains(selectCharacter))
            {
                return;
            }
            
            SelectedCharacters.Add(selectCharacter);
            
            CharacterSelected.Invoke(selectCharacter);
        }

        public void ClearSelectedCharacter()
        {
            SelectedCharacters.Clear();
        }
    }
}