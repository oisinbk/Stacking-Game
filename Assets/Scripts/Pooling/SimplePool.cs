using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public class SimplePool<T> : MonoSingleton<SimplePool<T>> where T : MonoBehaviour, IPoolable
    {
        [SerializeField] private T prefab;
        [SerializeField] private int size;
    
        private readonly Stack<T> _available = new();
        private readonly HashSet<T> _inUse = new();

        private void Awake()
        {
            for (int i = 0; i < size; i++)
            {
                var instance =  Instantiate(prefab, parent: transform);
                Return(instance);
            }
        }

        public T Get()
        {
            if (_available.Count < 1)
            {
                IncreasePool();
            }
            
            var pooledObject = _available.Pop();
            pooledObject.Reset();

            _inUse.Add(pooledObject);
            pooledObject.gameObject.SetActive(true);

            return pooledObject;
        }

        private void IncreasePool()
        {
            for (var i = 0; i < size; i++)
            {
                var instance =  Instantiate(prefab, parent: transform);
                Return(instance);
            }

            size *= 2;
        }

        public void Return(T obj)
        {
            if (_inUse.Contains(obj))
                _inUse.Remove(obj);
        
            obj.gameObject.SetActive(false);
            _available.Push(obj);
        }

        protected void ReturnAll()
        {
            var allObjects = new List<T>(_inUse);

            foreach (var obj in allObjects)
            {
                Return(obj);
            }
        
            _inUse.Clear();
        }
    }
}