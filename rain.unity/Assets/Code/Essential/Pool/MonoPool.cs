using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Essential
{
    [Serializable]
    public class MonoPool<T> where T: Mono, IPoolEntity
    {
        [SerializeField] private Transform _root;
        [SerializeField] private T _prefab;
        [SerializeField] private List<T> _all = new();
        [SerializeField] private List<T> _enabled = new();

        public T GetNext()
        {
            T entity = GetDisabledEntity() ?? AddNewEntity();
            _enabled.Add(entity);
            entity.Enable();
            return entity;
        }

        private T GetDisabledEntity()
        {
            return _all.FirstOrDefault(entity => entity != null && !entity.gameObject.activeSelf);
        }

        private T AddNewEntity()
        {
            T entity = GameObject.Instantiate(_prefab, _root);
            _all.Add(entity);
            return entity;
        }

        public IEnumerable<T> GetAll()
        {
            return _all;
        }

        public IEnumerable<T> GetAllEnabled()
        {
            return _enabled;
        }

        public void Disable(T entity)
        {
            if (entity == null || !entity.gameObject.activeSelf) return;
            entity.Disable();
            _enabled.Remove(entity);
        }

        public void DisableAll()
        {
            foreach (T entity in _all)
            {
                entity.Disable();
            }

            _enabled.Clear();
        }

        public T GetByIndex(int tabIndex)
        {
            return _all[tabIndex];
        }
    }
}