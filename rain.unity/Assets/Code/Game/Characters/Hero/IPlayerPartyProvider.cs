using System;
using System.Collections.Generic;

namespace Code.Game.Characters.Hero
{
    public interface IPlayerPartyProvider
    {
        event Action<CharacterView> CharacterAdded;
        List<CharacterView> Characters { get; }
        List<CharacterView> SelectedCharacters { get; }

        CharacterView CurrentCharacter { get; }
        
        void AddCharacter(CharacterView characterView);
    }
}