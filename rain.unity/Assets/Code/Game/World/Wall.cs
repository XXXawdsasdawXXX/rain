using System.Collections.Generic;
using TriInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Game.World
{
    public class Wall : Essential.Mono
    {
        [SerializeField] private Transform _root;
        [SerializeField] private Sprite[] _wallSprites;
        [SerializeField] private BoxCollider2D _heroesTrigger;
        [SerializeField] private int _lenght = 1;
        [SerializeField] private Vector2 _pieceSize = new Vector2(128, 258);
        [SerializeField] private List<SpriteRenderer> _renderers;
        [SerializeField] private float _hideAlpha = 0.4f;

        private readonly List<Collider2D> _insiders = new List<Collider2D>();
        
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            _insiders.Add(col);
            
            foreach (SpriteRenderer spriteRenderer in _renderers)
            {
                spriteRenderer.color = new Color(1, 1, 1, _hideAlpha);
            }
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            _insiders.Remove(other);

            if (_insiders.Count == 0)
            {
                foreach (SpriteRenderer spriteRenderer in _renderers)
                {
                    spriteRenderer.color = new Color(1, 1, 1, 1);
                }
            }
        }

#if UNITY_EDITOR

        [Button]
        private void _setLenght(int lenght)
        {
            _lenght = lenght;

            for (int i = _renderers.Count - 1; i >= 1; i--)
            {
                DestroyImmediate(_renderers[i].gameObject);
                _renderers.RemoveAt(i);
            }
            

            for (int i = 1; i < _lenght; i++)
            {
                SpriteRenderer render = Instantiate(_renderers[0], _root);
                render.transform.localPosition = new Vector3(_pieceSize.x * i  * 0.01f, 0);
                render.sprite = _wallSprites[Random.Range(0, _wallSprites.Length)];
                _renderers.Add(render);
            }
            
            _heroesTrigger.size = new Vector2(_pieceSize.x * lenght * 0.01f, _heroesTrigger.size.y);
            float offset = _pieceSize.x * 0.01f * 0.5f * (_lenght - 1);
            _heroesTrigger.offset = new Vector2(offset, _heroesTrigger.offset.y);
            
            EditorUtility.SetDirty(this);
        }
        
#endif

    }
}