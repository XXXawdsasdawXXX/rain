using System;
using System.Collections.Generic;

namespace Code.Game.Characters.Hero
{
    public interface IPlayerPartyProvider
    {
        event Action<CharacterActor> CharacterAdded;
        event Action<CharacterActor> HeroSelected;
        List<CharacterActor> Heroes { get; }
        CharacterActor CurrentCharacter { get; }
        
        void AddHero(CharacterActor character);
        void SetCurrentCharacter(CharacterActor selectCharacter);
        bool TryGet(string heroID, out CharacterActor actor);
    }
}