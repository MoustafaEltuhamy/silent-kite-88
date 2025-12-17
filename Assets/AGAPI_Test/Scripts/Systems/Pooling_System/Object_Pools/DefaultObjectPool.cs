using UnityEngine;

namespace AGAPI.Systems
{
    public class DefaultObjectPool<T> : ObjectPool<T> where T : MonoBehaviour, IPoolable<T>
    {

        public DefaultObjectPool(T prefab) : base(prefab)
        {
        }

        public override void PrewarmObject(int count)
        {
            count = Mathf.Max(1, count);

            for (int i = 0; i < count; i++)
            {
                T newObj = Object.Instantiate(_prefab);
                newObj.gameObject.SetActive(false);
                newObj.Pool = this;
                _objects.Enqueue(newObj);
            }
        }

        public override T GetObject(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            // true if we reused an existing object, false if we just instantiated one
            bool fromPool = _objects.Count > 0;

            T obj = fromPool
                ? _objects.Dequeue()
                : Object.Instantiate(_prefab, position, rotation, parent);

            if (obj.Pool == null)
            {
                obj.Pool = this;
                obj.gameObject.SetActive(false);
            }

            if (fromPool)
            {
                obj.transform.SetParent(parent);
                obj.transform.SetPositionAndRotation(position, rotation);
            }

            obj.gameObject.SetActive(true);
            obj.OnGetFromPool();
            return obj;
        }
        public override T GetObject(Transform parent = null)
        {
            bool fromPool = _objects.Count > 0;

            T obj;

            if (fromPool)
            {
                obj = _objects.Dequeue();

                if (parent != null)
                {
                    // Keep local position/rotation/scale relative to new parent
                    obj.transform.SetParent(parent, worldPositionStays: false);
                }
            }
            else
            {
                obj = parent != null
                    ? Object.Instantiate(_prefab, parent)
                    : Object.Instantiate(_prefab);
            }

            if (obj.Pool == null)
            {
                obj.Pool = this;
                obj.gameObject.SetActive(false);
            }

            obj.gameObject.SetActive(true);
            obj.OnGetFromPool();
            return obj;
        }

        public override void ReturnObject(T obj)
        {
            obj.OnReturnToPool();
            obj.gameObject.SetActive(false);
            _objects.Enqueue(obj);
        }
    }
}
