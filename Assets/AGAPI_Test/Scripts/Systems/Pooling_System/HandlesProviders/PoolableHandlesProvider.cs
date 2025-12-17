using System;
using System.Collections.Generic;
using UnityEngine;

namespace AGAPI.Systems
{
    public class PoolableHandlesProvider : IPoolableHandlesProvider
    {
        readonly Dictionary<int, object> _prefabsHandlesCache = new Dictionary<int, object>();

        public PoolableHandle<T> GetHandle<T>(T prefab) where T : MonoBehaviour, IPoolable<T>
        {
            int key = prefab.GetInstanceID();
            if (!_prefabsHandlesCache.TryGetValue(key, out var PoolableHandleObj))
            {
                var poolableHandle = new PoolableHandle<T>(prefab, key);
                _prefabsHandlesCache[key] = poolableHandle;
                return poolableHandle;
            }
            return (PoolableHandle<T>)PoolableHandleObj;
        }
    }
}
