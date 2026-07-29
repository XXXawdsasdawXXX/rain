using System;
using System.Collections.Generic;

namespace Code.Game.Characters.Hero
{
    public interface IPlayerPartyProvider
    {
        event Action<CharacterView> CharacterAdded;
        event Action<CharacterView> CharacterSelected;
        List<CharacterView> Characters { get; }
        List<CharacterView> SelectedCharacters { get; }
        CharacterView CurrentCharacter { get; }
        
        void AddHero(CharacterView characterView);
        void AddSelectCharacter(CharacterView selectCharacter);
        void ClearSelectedCharacter();
    }
}