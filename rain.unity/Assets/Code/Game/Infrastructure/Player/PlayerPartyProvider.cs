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
        public event Action<CharacterActor> CharacterAdded;
        public event Action<CharacterActor> HeroSelected;
        public List<CharacterActor> Heroes { get; } = new();
        public CharacterActor CurrentCharacter { get; private set; }

        

        public void AddHero(CharacterActor character)
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

        public void SetCurrentCharacter(CharacterActor selectCharacter)
        {
            if (CurrentCharacter != null)
            {
                CurrentCharacter.SetSelected(false);
            }

            CurrentCharacter = selectCharacter;
            CurrentCharacter.SetSelected(true);
            
            HeroSelected?.Invoke(CurrentCharacter);
        }

        public bool TryGet(string heroID, out CharacterActor actor)
        {
            actor = Heroes.FirstOrDefault(h => h.Model.ID == heroID);
            return actor != null;
        }
    }
}