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
        public event Action<CharacterWorldActor> CharacterAdded;
        public event Action<CharacterWorldActor> HeroSelected;
        public List<CharacterWorldActor> Heroes { get; } = new();
        public CharacterWorldActor CurrentCharacter { get; private set; }

        

        public void AddHero(CharacterWorldActor character)
        {
            if (Heroes.Exists(c => c.Model.ID == character.Model.ID))
            {
                Log.Error(this, "Character already exist into collection");
                return;
            }
            
            Heroes.Add(character);
            
            CharacterAdded?.Invoke(character);

            if (Heroes.Count == 1)
            {
                SetCurrentCharacter(character);
            }
        }

        public void SetCurrentCharacter(CharacterWorldActor selectCharacter)
        {
            if (CurrentCharacter != null)
            {
                CurrentCharacter.SetSelected(false);
            }

            CurrentCharacter = selectCharacter;
            CurrentCharacter.SetSelected(true);
            
            HeroSelected?.Invoke(CurrentCharacter);
        }

        public bool TryGet(string heroID, out CharacterWorldActor actor)
        {
            actor = Heroes.FirstOrDefault(h => h.Model.ID == heroID);
            return actor != null;
        }
    }
}