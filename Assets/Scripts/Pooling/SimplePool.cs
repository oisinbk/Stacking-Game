using System.Collections.Generic;
using System.Diagnostics;
using DebugTools;
using UnityEngine;

namespace Pooling
{
    public class SimplePool<T> : MonoBehaviour where T : MonoBehaviour, IPoolable
    {
        [SerializeField] private T prefab;
        [SerializeField] private int size;
    
        [Header("Settings")]
        [SerializeField] private int originalSize = 5;
        [SerializeField] private int increaseSize = 5;
        
        private readonly Stack<T> _available = new();
        private readonly HashSet<T> _inUse = new();

        private void Awake()
        {
            IncreasePool(originalSize);
        }

        public T Get()
        {
            // #region agent log
            bool poolWasEmpty = _available.Count < 1;
            // #endregion
            if (_available.Count < 1)
            {
                // #region agent log
                var expandSw = Stopwatch.StartNew();
                int sizeBefore = size;
                // #endregion
                IncreasePool(increaseSize);
                // #region agent log
                expandSw.Stop();
                AgentDebugLog.Write(
                    "SimplePool.cs:Get:expand",
                    "Pool expanded",
                    "B",
                    dataJson: $"{{\"expandMs\":{expandSw.ElapsedMilliseconds},\"sizeBefore\":{sizeBefore},\"sizeAfter\":{size},\"availableAfter\":{_available.Count}}}");
                // #endregion
            }
            
            var pooledObject = _available.Pop();
            pooledObject.Reset();

            _inUse.Add(pooledObject);
            pooledObject.gameObject.SetActive(true);

            return pooledObject;
        }

        private void IncreasePool(int amount)
        {
            size += amount;
            
            for (var i = 0; i < size; i++)
            {
                var instance =  Instantiate(prefab, parent: transform);
                Return(instance);
            }
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