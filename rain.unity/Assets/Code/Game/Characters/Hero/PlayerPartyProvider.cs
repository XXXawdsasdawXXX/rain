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
        public List<CharacterView> Characters { get; } = new();
        public List<CharacterView> SelectedCharacters { get; } = new List<CharacterView>();
        public CharacterView CurrentCharacter { get; }


        public void AddCharacter(CharacterView characterView)
        {
            if (Characters.Contains(characterView))
            {
                Log.Error(this, "Character already exist into collection");
                return;
            }
            
            Characters.Add(characterView);
            
            //todo remove. now for test
            SelectedCharacters.Add(characterView);
            
            CharacterAdded?.Invoke(characterView);
        }
    }
}