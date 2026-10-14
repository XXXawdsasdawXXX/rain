using System;
using System.Collections.Generic;

namespace Code.Game.Characters.Hero
{
    public interface IPlayerPartyProvider
    {
        event Action<CharacterWorldActor> CharacterAdded;
        event Action<CharacterWorldActor> HeroSelected;
        List<CharacterWorldActor> Heroes { get; }
        CharacterWorldActor CurrentCharacter { get; }
        
        void AddHero(CharacterWorldActor character);
        void SetCurrentCharacter(CharacterWorldActor selectCharacter);
        bool TryGet(string heroID, out CharacterWorldActor actor);
    }
}