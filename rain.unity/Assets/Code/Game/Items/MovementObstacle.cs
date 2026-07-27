using System;
using Code.Pathfinding;
using UnityEngine;

namespace Code.Game.Items
{
    public class MovementObstacle : MonoBehaviour, IPathfindingObstacle
    {
        public bool BlocksPathfinding => true;

        [SerializeField] private float _speed = 4;
        [SerializeField] private float _distance;

        private float _startAxisPosition;
        private bool _moveUp;
        
        
        private void Start()
        {
            _startAxisPosition = transform.position.y;
        }

        private void Update()
        {
            if (!_moveUp)
            {
                if (transform.position.y < _startAxisPosition)
                {
                    _moveUp = true;
                }
            }
            else
            {
                if (transform.position.y > _startAxisPosition + _distance)
                {
                    _moveUp = false;
                }
            }

            transform.position += new Vector3(0, (_moveUp ? _speed : -_speed) * Time.deltaTime, 0);
        }
    }
}