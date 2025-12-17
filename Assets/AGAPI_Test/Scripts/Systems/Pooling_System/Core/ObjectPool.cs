
using System.Collections.Generic;
using UnityEngine;

namespace AGAPI.Systems
{
    public abstract class ObjectPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        protected readonly Queue<T> _objects = new Queue<T>();
        protected readonly T _prefab;

        public ObjectPool(T prefab)
        {
            _prefab = prefab;

            if (_prefab == null)
                Debug.LogError("Prefab is null in ObjectPool");
        }

        public abstract void PrewarmObject(int count);

        public abstract T GetObject(Vector3 position, Quaternion rotation, Transform parent = null);
        public abstract T GetObject(Transform parent = null);

        public abstract void ReturnObject(T obj);
    }
}