using Code.Game.Factories;
using Code.Game.Save;
using TriInspector;
using UnityEngine;
using VContainer;

namespace Code.Test
{
    public class TestParty : MonoBehaviour
    {
        [SerializeField] private CharacterModel _characterModel;
        private ICharacterFactory _characterFactory;


        [Inject]
        public void Construct(ICharacterFactory characterFactory)
        {
            _characterFactory = characterFactory;
        }
        
        [Button]
        private void _addCharacter()
        {
            _characterFactory.SpawnHero(_characterModel);
        }
    }
}
