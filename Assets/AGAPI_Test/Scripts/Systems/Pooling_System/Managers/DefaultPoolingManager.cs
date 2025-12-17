using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGAPI.Systems
{
    public class DefaultPoolingManager : IPoolingManager
    {
        private readonly Dictionary<int, object> _pools = new Dictionary<int, object>();

        public PoolableHandlesProvider DefaultHandelsProvider { get; private set; }


        public DefaultPoolingManager(PoolableHandlesProvider defaultHandelsProvider)
        {
            DefaultHandelsProvider = defaultHandelsProvider;
        }

        public void CreatePrewarmObject<T>(PoolableHandle<T> handle, int count) where T : MonoBehaviour, IPoolable<T>
        {
            if (count <= 0) return;
            if (handle == null) throw new ArgumentNullException(nameof(handle));
            if (handle.Prefab == null) throw new ArgumentException("Handle.Prefab is null.", nameof(handle));

            int key = handle.Key;

            if (!_pools.TryGetValue(key, out var existingPool))
            {
                var newPool = new DefaultObjectPool<T>(handle.Prefab);
                _pools[key] = newPool;
                newPool.PrewarmObject(count);
                return;
            }

            // Safer cast with validation
            if (existingPool is ObjectPool<T> typedPool)
            {
                typedPool.PrewarmObject(count);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Pool with key {key} exists but is not an ObjectPool<{typeof(T).Name}> (actual: {existingPool?.GetType().Name}).");
            }
        }

        public T GetPooledObject<T>(PoolableHandle<T> poolableHandel, Vector3 position, Quaternion rotation, Transform parent = null) where T : MonoBehaviour, IPoolable<T>
        {
            int key = poolableHandel.Key;
            if (_pools.TryGetValue(key, out var existingPool))
            {
                return ((ObjectPool<T>)existingPool).GetObject(position, rotation, parent);
            }

            var pool = new DefaultObjectPool<T>(poolableHandel.Prefab);
            _pools[key] = pool;

            return pool.GetObject(position, rotation, parent);
        }
        public T GetPooledObject<T>(PoolableHandle<T> poolableHandel, Transform parent = null) where T : MonoBehaviour, IPoolable<T>
        {
            int key = poolableHandel.Key;
            if (_pools.TryGetValue(key, out var existingPool))
            {
                return ((ObjectPool<T>)existingPool).GetObject(parent);
            }

            var pool = new DefaultObjectPool<T>(poolableHandel.Prefab);
            _pools[key] = pool;

            return pool.GetObject(parent);
        }

    }
}